using SX.WebCore.Abstract;
using SX.WebCore.MvcControllers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.SessionState;

namespace SX.WebCore.MvcControllers
{
    [Authorize(Roles = "seo")]
    [SessionState(SessionStateBehavior.Default)]
    public abstract class SxSeoWordCounterController :SxBaseController
    {
        private ISxSeoWordCounter _counter;

        private List<SxSeoPhrase> _data
        {
            get
            {
                var data = Session[Session.SessionID + "_seo_phrases"] as List<SxSeoPhrase>;
                if (data == null)
                    Session[Session.SessionID + "_seo_phrases"] = new List<SxSeoPhrase>();
                return Session[Session.SessionID + "_seo_phrases"] as List<SxSeoPhrase>;
            }
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(_data.ToArray());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual PartialViewResult AddPhrases(string text)
        {
            string[] stringSeparators = new string[] { "\r\n" };
            string[] phrases = text.Split(stringSeparators, StringSplitOptions.None);
            for (int i = 0; i < phrases.Length; i++)
            {
                var phrase = phrases[i];
                if (!string.IsNullOrEmpty(phrase) && _data.SingleOrDefault(x => x.Text == phrase) == null)
                {
                    var model = new SxSeoPhrase(phrase);
                    _counter = new Managers.SxSeoWordCounter();
                    model.WordCount = _counter.GetWordCount(model);
                    _data.Add(model);
                }
            }

            return PartialView("_Table", _data.ToArray());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual FileResult Report()
        {
            var sb = new StringBuilder();
            foreach (var item in _data)
            {
                var str = item.Text.Trim().Split(' ');
                var res = string.Empty;
                for (int i = 0; i < str.Length; i++)
                {
                    res += "," + str[i];
                }
                sb.AppendLine(res.Substring(1) + " [" + item.WordCount + "]");
            }
            int pageCode = 1251;
            Encoding encoding = Encoding.GetEncoding(pageCode);
            byte[] encodedBytes = encoding.GetBytes(sb.ToString());
            return File(encodedBytes, "text/csv", "seo-words-count.csv");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual PartialViewResult Delete()
        {
            Session[Session.SessionID + "_seo_phrases"] = null;
            return PartialView("_Table", _data.ToArray());
        }
    }
}
