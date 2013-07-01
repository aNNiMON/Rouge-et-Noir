using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Model {

    /// <summary>
    /// Класс вспомогательных функций.
    /// </summary>
    public static class Util {

        private static readonly Random rnd = new Random();

        public static Random GetRandom() {
            return rnd;
        }

        /// <summary>
        /// Перемешать данные в списке.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list) {
            int size = list.Count;
            while (size > 1) {
                size--;
                int index = rnd.Next(size + 1);
                T value = list[index];
                list[index] = list[size];
                list[size] = value;
            }
        }

        /// <summary>
        /// Переместить объекты из одного списка в другой.
        /// Берутся объекты из конца исходного списка
        /// и добавляются в конец результирующего.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="from">список, из которого нужно переместить объекты</param>
        /// <param name="to">результирующий список</param>
        /// <param name="length">количество объектов для перемещения</param>
        public static void Move<T>(List<T> from, List<T> to, int length) {
            var selected = from.Skip(Math.Max(0, from.Count() - length)).Take(length).ToList();
            selected.ForEach(item => from.Remove(item));
            to.AddRange(selected);
        }

        /// <summary>
        /// Получение текста из интернета по URL.
        /// </summary>
        /// <param name="url">Ссылка, по которой необходимо взять информацию</param>
        /// <param name="encoding">Кодировка текста</param>
        /// <returns></returns>
        public static string GetTextFromUrl(string url, Encoding encoding = null) {
            string text = "";
            var req = (HttpWebRequest) WebRequest.Create(url);
            HttpWebResponse response = null;
            try {
                response = (HttpWebResponse) req.GetResponse();
                Stream stream = response.GetResponseStream();
                if (encoding == null)
                    encoding = Encoding.Default;
                if (stream != null) {
                    var streamReader = new StreamReader(stream, encoding);
                    text = streamReader.ReadToEnd();
                    streamReader.Close();
                }
            } catch (Exception ex) {
                text = "";
            } finally {
                if (response != null) response.Close();
            }
            return text;
        }

        /// <summary>
        /// Создание POST-запроса и передача параметров.
        /// </summary>
        /// <param name="url">адрес</param>
        /// <param name="value">параметры (ключ-значение)</param>
        /// <returns></returns>
        public static HttpWebRequest CreateRequest(String url, NameValueCollection value) {
            var parameters = new StringBuilder();
            foreach (string key in value) {
                parameters.AppendFormat("&{0}={1}",
                    HttpUtility.UrlEncode(key),
                    HttpUtility.UrlEncode(value[key]));
            }

            var request = (HttpWebRequest) HttpWebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            using (var writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8)) {
                writer.Write(parameters.ToString());
            }

            return request;
        }

        /// <summary>
        /// Перевод Unix timestamp в DateTime.
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
