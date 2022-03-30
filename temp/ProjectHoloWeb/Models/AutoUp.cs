using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectHoloWeb.Models
{
    public class AutoUp
    {
        public AutoUp()
        {
        }

        public string lastCode { set; get; }
        public string nameCode { set; get; }
        public int lenthNum { set; get; }

        public AutoUp(string lastCode, string nameCode, int lenthNum)
        {
            this.lastCode = lastCode;
            this.nameCode = nameCode;
            this.lenthNum = lenthNum;
        }

        public string CreateCodeAuto()
        {
            string kq = "";

            if(lastCode == null)
            {
                string numLast = "";
                int i = 0;
                while (i < lenthNum)
                {
                    numLast = numLast + "0";
                    i++;
                }
                kq = nameCode + numLast;
            }
            else
            {
                string numLast = lastCode.Substring(nameCode.Length);
                int covertNum = int.Parse(numLast);
                covertNum++;

                kq = nameCode;
                int n = lenthNum - covertNum.ToString().Length;
                int i = 0;
                while (i < n)
                {
                    kq = kq + "0";
                    i++;
                }
                kq = kq + covertNum.ToString();
            }

            return kq;
        }
    }
}