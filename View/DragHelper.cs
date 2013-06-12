using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace View {

    /// <summary>
    /// Вспомогательный класс для облегчения перемещения компонент.
    /// </summary>
    public static class DragHelper {

        public static void Drag(this FrameworkElement attachedElement, MouseButtonEventHandler onDragCompleted, Point pos) {
            bool isDragging = true;
            var lastPosition = pos;
            attachedElement.CaptureMouse();

            // Кнопка мыши отпущена - перемещение закончено.
            attachedElement.MouseLeftButtonUp += (s, e) => {
                isDragging = false;
                attachedElement.ReleaseMouseCapture();

                if (onDragCompleted != null)
                    onDragCompleted(s, e);
            };

            // Перемещение компонента через RenderTransform.
            attachedElement.MouseMove += (s, e) => {
                if (!isDragging)
                    return;

                Point currentPosition = e.GetPosition(null);

                double dX = currentPosition.X - lastPosition.X;
                double dY = currentPosition.Y - lastPosition.Y;

                lastPosition = currentPosition;

                var oldTransform = attachedElement.RenderTransform;
                var rt = new TransformGroup();
                var newPos = new TranslateTransform {
                    X = dX,
                    Y = dY
                };

                if (oldTransform != null) {
                    rt.Children.Add(oldTransform);
                }
                rt.Children.Add(newPos);

                var mt = new MatrixTransform {Matrix = rt.Value};
                attachedElement.RenderTransform = mt;

            };
        }
    }
}
