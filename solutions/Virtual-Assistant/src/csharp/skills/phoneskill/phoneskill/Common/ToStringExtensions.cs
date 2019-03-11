using System.Collections.Generic;
using System.Text;

namespace PhoneSkill.Common
{
    public static class ToStringExtensions
    {
        public static string ToPrettyString<T>(this IList<T> list)
        {
            var builder = new StringBuilder();
            builder.Append("[");

            var isFirst = true;
            foreach (var element in list)
            {
                if (!isFirst)
                {
                    builder.Append(", ");
                }

                var elementList = element as IList<object>;
                if (elementList != null)
                {
                    builder.Append(elementList.ToPrettyString());
                }
                else
                {
                    builder.Append(element);
                }

                isFirst = false;
            }

            builder.Append("]");
            return builder.ToString();
        }
    }
}
