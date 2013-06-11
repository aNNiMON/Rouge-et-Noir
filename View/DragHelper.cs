using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace View {

    public static class DragHelper {

        public static void Drag(this FrameworkElement attachedElement, MouseButtonEventHandler onDragCompleted) {
            bool isDragging = false;
            Point lastPosition = new Point(0, 0);

            int zIndex = Panel.GetZIndex(attachedElement);

            attachedElement.MouseLeftButtonDown += (s, e) => {
                isDragging = true;
                lastPosition = e.GetPosition(null);
                attachedElement.CaptureMouse();

                Panel.SetZIndex(attachedElement, Util.GetMaxZIndex(GameView.Instance.GetRootViewElements()));
            };

            attachedElement.MouseLeftButtonUp += (s, e) => {
                isDragging = false;
                attachedElement.ReleaseMouseCapture();

                Panel.SetZIndex(attachedElement, zIndex);

                if (onDragCompleted != null)
                    onDragCompleted(s, e);
            };

            attachedElement.MouseMove += (s, e) => {
                if (!isDragging) return;

                Point currentPosition = e.GetPosition(null);

                double dX = currentPosition.X - lastPosition.X;
                double dY = currentPosition.Y - lastPosition.Y;

                lastPosition = currentPosition;

                Transform oldTransform = attachedElement.RenderTransform;
                TransformGroup rt = new TransformGroup();
                TranslateTransform newPos = new TranslateTransform();
                newPos.X = dX;
                newPos.Y = dY;

                if (oldTransform != null) {
                    rt.Children.Add(oldTransform);
                }
                rt.Children.Add(newPos);

                MatrixTransform mt = new MatrixTransform();
                mt.Matrix = rt.Value;

                attachedElement.RenderTransform = mt;

            };
        }
    }
}
