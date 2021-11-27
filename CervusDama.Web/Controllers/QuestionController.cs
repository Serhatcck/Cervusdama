using CervusDama.Authorization;
using CervusDama.Data.Model.QuestionModel;
using CervusDama.Data.Model.TicketModel;
using CervusDama.Utility.PagerHelper;
using CervusDama.Utility.SecurityHelper;
using CervusDama.Utility.StringHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
    public class QuestionController : Base.BaseController
    {

        public ActionResult Index(string search,bool desc=true)
        {
            List<QuestionListModel> questionList = new List<QuestionListModel>();
           
            int getDataSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["paging"]);
            if (String.IsNullOrEmpty(search))
            {
                if(desc)
                {
                    questionList = dbContext.Question.Where(q => q.Status == (byte)Data.QuestionStatus.Active || q.Status == (byte)Data.QuestionStatus.Solved)
                    .OrderByDescending(o => o.CreateAt).ToPagedList(1, getDataSize)
                    .Select(s => new QuestionListModel()
                    {
                        ID = s.ID,
                        FirstName = s.User.FirstName,
                        LastName = s.User.LastName,
                        Title = s.Title,
                        UserSlug = s.User.Slug,
                        Hit = s.Hit,
                        //Content = HttpUtility.HtmlDecode(s.Content),
                        Content = s.Content,
                        CreateAt = s.CreateAt,
                        Slug = s.Slug,
                        Status = s.Status,
                        AnswerCount = dbContext.Answer.Count(a => a.QuestionID == s.ID),
                        Tickets = s.Tickets.Select(t => new TicketListModel() { Title = t.Title, Slug = t.Slug }).ToList(),
                        IsSolved = s.IsSolved
                    }).ToList();
                }else
                {
                    questionList = dbContext.Question.Where(q => q.Status == (byte)Data.QuestionStatus.Active || q.Status == (byte)Data.QuestionStatus.Solved)
                    .OrderBy(o => o.CreateAt).ToPagedList(1, getDataSize)
                    .Select(s => new QuestionListModel()
                    {
                        ID = s.ID,
                        FirstName = s.User.FirstName,
                        LastName = s.User.LastName,
                        Title = s.Title,
                        UserSlug = s.User.Slug,
                        Hit = s.Hit,
                        //Content = HttpUtility.HtmlDecode(s.Content),
                        Content = s.Content,
                        CreateAt = s.CreateAt,
                        Slug = s.Slug,
                        Status = s.Status,
                        AnswerCount = dbContext.Answer.Count(a => a.QuestionID == s.ID),
                        Tickets = s.Tickets.Select(t => new TicketListModel() { Title = t.Title, Slug = t.Slug }).ToList(),
                        IsSolved = s.IsSolved
                    }).ToList();
                }
            }
            else
            {
                if(desc)
                {
                    var searchSlug = StringHelper.Slug(search);
                    //|| q.Title.Contains(search) ||q.Content.Contains(search)
                    questionList = dbContext.Question
                        .Where(q => (q.Status == 1 || q.Status == 0) && (q.Slug.Equals(searchSlug) || q.Title.Contains(search) || q.Content.Contains(search)))
                        .OrderByDescending(o => o.CreateAt).ToPagedList(1, getDataSize).Select(s => new QuestionListModel()
                        {
                            ID = s.ID,
                            FirstName = s.User.FirstName,
                            LastName = s.User.LastName,
                            Title = s.Title,
                            UserSlug = s.User.Slug,
                        //Content = HttpUtility.HtmlDecode(s.Content),
                        Content = s.Content,
                            CreateAt = s.CreateAt,
                            Slug = s.Slug,
                            Status = s.Status,
                            AnswerCount = dbContext.Answer.Count(a => a.QuestionID == s.ID),
                            Tickets = s.Tickets.Select(t => new TicketListModel() { Title = t.Title, Slug = t.Slug }).ToList()
                        }).ToList();
                }else
                {
                    var searchSlug = StringHelper.Slug(search);
                    //|| q.Title.Contains(search) ||q.Content.Contains(search)
                    questionList = dbContext.Question
                        .Where(q => (q.Status == 1 || q.Status == 0) && (q.Slug.Equals(searchSlug) || q.Title.Contains(search) || q.Content.Contains(search)))
                        .OrderBy(o => o.CreateAt).ToPagedList(1, getDataSize).Select(s => new QuestionListModel()
                        {
                            ID = s.ID,
                            FirstName = s.User.FirstName,
                            LastName = s.User.LastName,
                            Title = s.Title,
                            UserSlug = s.User.Slug,
                        //Content = HttpUtility.HtmlDecode(s.Content),
                        Content = s.Content,
                            CreateAt = s.CreateAt,
                            Slug = s.Slug,
                            Status = s.Status,
                            AnswerCount = dbContext.Answer.Count(a => a.QuestionID == s.ID),
                            Tickets = s.Tickets.Select(t => new TicketListModel() { Title = t.Title, Slug = t.Slug }).ToList()
                        }).ToList();
                }
            }
            ViewBag.Search= search ?? "" ;
            ViewBag.desc = desc; 
            return View(questionList);
        }

        public JsonResult IndexLazyLoad(string search,bool desc,int page)
        {
            List<QuestionListModel> questionList = new List<QuestionListModel>();
            int getDataSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["paging"]);
            if (String.IsNullOrEmpty(search))
            {
                if (desc)
                {
                    questionList = dbContext.Question.Where(q => q.Status == (byte)Data.QuestionStatus.Active || q.Status == (byte)Data.QuestionStatus.Solved)
                    .OrderByDescending(o => o.CreateAt).ToPagedList(page, getDataSize)
                    .Select(s => new QuestionListModel()
                    {
                        ID = s.ID,
                        FirstName = s.User.FirstName,
                        LastName = s.User.LastName,
                        Title = s.Title,
                        UserSlug = s.User.Slug,
                        Hit = s.Hit,
                        //Content = HttpUtility.HtmlDecode(s.Content),
                        Content = s.Content,
                        CreateAt = s.CreateAt,
                        Slug = s.Slug,
                        Status = s.Status,
                        AnswerCount = dbContext.Answer.Count(a => a.QuestionID == s.ID),
                        Tickets = s.Tickets.Select(t => new TicketListModel() { Title = t.Title, Slug = t.Slug }).ToList(),
                        IsSolved = s.IsSolved
                    }).ToList();
                }
                else
                {
                    questionList = dbContext.Question.Where(q => q.Status == (byte)Data.QuestionStatus.Active || q.Status == (byte)Data.QuestionStatus.Solved)
                    .OrderBy(o => o.CreateAt).ToPagedList(page, getDataSize)
                    .Select(s => new QuestionListModel()
                    {
                        ID = s.ID,
                        FirstName = s.User.FirstName,
                        LastName = s.User.LastName,
                        Title = s.Title,
                        UserSlug = s.User.Slug,
                        Hit = s.Hit,
                        //Content = HttpUtility.HtmlDecode(s.Content),
                        Content = s.Content,
                        CreateAt = s.CreateAt,
                        Slug = s.Slug,
                        Status = s.Status,
                        AnswerCount = dbContext.Answer.Count(a => a.QuestionID == s.ID),
                        Tickets = s.Tickets.Select(t => new TicketListModel() { Title = t.Title, Slug = t.Slug }).ToList(),
                        IsSolved = s.IsSolved
                    }).ToList();
                }
            }
            else
            {
                if (desc)
                {
                    var searchSlug = StringHelper.Slug(search);
                    //|| q.Title.Contains(search) ||q.Content.Contains(search)
                    questionList = dbContext.Question
                        .Where(q => (q.Status == 1 || q.Status == 0) && (q.Slug.Equals(searchSlug) || q.Title.Contains(search) || q.Content.Contains(search)))
                        .OrderByDescending(o => o.CreateAt).ToPagedList(page, getDataSize).Select(s => new QuestionListModel()
                        {
                            ID = s.ID,
                            FirstName = s.User.FirstName,
                            LastName = s.User.LastName,
                            Title = s.Title,
                            UserSlug = s.User.Slug,
                            //Content = HttpUtility.HtmlDecode(s.Content),
                            Content = s.Content,
                            CreateAt = s.CreateAt,
                            Slug = s.Slug,
                            Status = s.Status,
                            AnswerCount = dbContext.Answer.Count(a => a.QuestionID == s.ID),
                            Tickets = s.Tickets.Select(t => new TicketListModel() { Title = t.Title, Slug = t.Slug }).ToList()
                        }).ToList();
                }
                else
                {
                    var searchSlug = StringHelper.Slug(search);
                    //|| q.Title.Contains(search) ||q.Content.Contains(search)
                    questionList = dbContext.Question
                        .Where(q => (q.Status == 1 || q.Status == 0) && (q.Slug.Equals(searchSlug) || q.Title.Contains(search) || q.Content.Contains(search)))
                        .OrderBy(o => o.CreateAt).ToPagedList(page, getDataSize).Select(s => new QuestionListModel()
                        {
                            ID = s.ID,
                            FirstName = s.User.FirstName,
                            LastName = s.User.LastName,
                            Title = s.Title,
                            UserSlug = s.User.Slug,
                            //Content = HttpUtility.HtmlDecode(s.Content),
                            Content = s.Content,
                            CreateAt = s.CreateAt,
                            Slug = s.Slug,
                            Status = s.Status,
                            AnswerCount = dbContext.Answer.Count(a => a.QuestionID == s.ID),
                            Tickets = s.Tickets.Select(t => new TicketListModel() { Title = t.Title, Slug = t.Slug }).ToList()
                        }).ToList();
                }
            }
            return Json(new { questions = questionList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(string slug)
        {
            if (String.IsNullOrEmpty(slug))
                return RedirectToRoute("Questions");

            if (!dbContext.Question.Any(a => a.Slug.Equals(slug)))
                return RedirectToRoute("Questions");

            //var article = dbContext.Article.Include("Tickets").FirstOrDefault(a => a.Slug.Equals(slug));
            var question = dbContext.Question.Include("Tickets").FirstOrDefault(q => q.Slug.Equals(slug));
            question.Hit = question.Hit + 1;
            dbContext.SaveChanges();

            QuestionDetailModel model = new QuestionDetailModel();
            model.ID = question.ID;
            model.Title = question.Title;
            model.Content = question.Content;
            model.CreateAt = question.CreateAt;
            model.Slug = question.Slug;
            model.Status = question.Status;
            model.LikeCount = dbContext.QuestionVoting.Count(v => v.QuestionID == question.ID && v.VoteType == true);
            model.DisLikeCount = dbContext.QuestionVoting.Count(v => v.QuestionID == question.ID && v.VoteType == false);
            /*
             model.Tickets = article.Tickets.Select(s => new Data.Model.TicketModel.TicketListModel()
			{
				ID = s.ID,
				Title = s.Title,
				Slug = s.Slug
			}).ToList();
             */
            model.Tickets = question.Tickets.Select(t => new Data.Model.TicketModel.TicketListModel()
            {
                ID = t.ID,
                Title = t.Title,
                Slug = t.Slug
            }).ToList();

            model.UserInfo = new Data.Model.UserModel.UserInfoModel()
            {
                ID = question.User.ID,
                FirstName = question.User.FirstName,
                LastName = question.User.LastName,
                Slug = question.User.Slug
            };
            model.Answers = dbContext.Answer.Where(a => a.QuestionID == question.ID).OrderBy(o => o.CreateAt).Select(s => new Data.Model.AnswerModel.QuestionAnswerModel()
            {
                ID = s.ID,
                Content = s.Content,
                CreateAt = s.CreateAt,
                LikeCount = dbContext.AnswerVoting.Count(v => v.AnswerID == s.ID && v.VoteType == true),
                DislikeCount = dbContext.AnswerVoting.Count(v => v.AnswerID == s.ID && v.VoteType == false),
                UserInfo = new Data.Model.UserModel.UserInfoModel()
                {
                    ID = s.User.ID,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    Slug = s.User.Slug
                },
                isSolved = s.isSolved
            }).ToList();

            if (TempData["message"] != null)
            {
                model.ActionResult = TempData["message"].ToString();
            }
            else
            {
                model.ActionResult = "";
            }

            return View(model);
        }

        [Authorization(Permission = "")]
        public ActionResult Insert()
        {
            return View();
        }

        [Authorization(Permission = "")]
        [ValidateInput(false)] //xss açığı kontrol.
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Insert(QuestionInsertModel data)
        {
            if (ModelState.IsValid)
            {
                SecurityHelper sch = new SecurityHelper();

                data.Content = data.Content.Replace(" style=\"text-align: start;\"", String.Empty);
                data.Content = data.Content.Replace("<br>", String.Empty);

                try
                {
                    data.Content = sch.XSSFilter(data.Content);
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, message = "Gönderilen Soru içeriği hatalı. " + ex.Message }, JsonRequestBehavior.AllowGet);
                }
                string questionSlug = StringHelper.Slug(data.Title);
                if (questionSlug.Length > 0)
                {
                    if (!dbContext.Question.Any(a => a.Title.Equals(data.Title) || a.Slug.Equals(questionSlug)))
                    {
                        var question = dbContext.Question.Create();
                        question.Slug = questionSlug;
                        question.Status = (byte)Data.QuestionStatus.Active;
                        question.Title = data.Title;
                        //question.Content = data.Content;
                        question.Content = data.Content.Replace("<#text>", String.Empty);
                        question.CreateAt = DateTime.Now;
                        question.UserID = int.Parse(User.Identity.Name);
                        if (!String.IsNullOrEmpty(data.Tickets))
                        {
                            foreach (var item in data.Tickets.Split(';'))
                            {
                                string ticketSlug = StringHelper.Slug(item);

                                if (dbContext.Ticket.Any(t => t.Title.Equals(item) && t.Slug.Equals(ticketSlug)))
                                {
                                    question.Tickets.Add(dbContext.Ticket.FirstOrDefault(t => t.Title.Equals(item) && t.Slug.Equals(ticketSlug)));
                                }
                                else
                                {
                                    question.Tickets.Add(new Data.Entities.Ticket { Title = item, Slug = ticketSlug });
                                }
                            }
                        }

                        dbContext.Question.Add(question);

                        try
                        {
                            dbContext.SaveChanges();
                            return Json(new { status = true, message = "Soru başarıyla eklendi.", slug = question.Slug }, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            return Json(new { status = false, message = "Kayıt işlemi sırasında bir hata oluştu." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { status = false, message = "Sistemde aynı başlığa kayıtlı bir soru mevcut. Sorunuza başka bir isim veriniz." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = false, message = "Soru için geçersiz başlık." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = false, message = string.Join("<br/>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorization(Permission = "")]
        public ActionResult Edit(string slug)
        {


            if (String.IsNullOrEmpty(slug))
                return RedirectToRoute("Questions");

            int userID = int.Parse(User.Identity.Name);

            if (!dbContext.Question.Any(a => a.Slug.Equals(slug) && a.UserID == userID))
                return RedirectToRoute("Questions");

            var question = dbContext.Question.Include("Tickets").FirstOrDefault(q => q.Slug.Equals(slug));
            QuestionEditModel model = new QuestionEditModel();
            model.ID = question.ID;
            model.Content = question.Content;
            model.Title = question.Title;
            model.Tickets = question.Tickets.Select(t => new Data.Model.TicketModel.TicketListModel()
            {
                ID = t.ID,
                Title = t.Title,
                Slug = t.Slug
            }).ToList();
            model.TicketList = String.Join(";", model.Tickets.Select(s => s.Title));
            return View(model);
        }

        [Authorization(Permission = "")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(QuestionInsertModel data)
        {
            if (ModelState.IsValid)
            {
                int userID = int.Parse(User.Identity.Name);
                if (dbContext.Question.Any(q => q.ID == data.ID && q.UserID == userID))
                {
                    SecurityHelper sch = new SecurityHelper();
                    data.Content = data.Content.Replace(" style=\"text-align: start;\"", String.Empty).Replace("<br>", String.Empty);
                    try
                    {
                        data.Content = sch.XSSFilter(data.Content);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { status = false, message = "Gönderilen Soru içeriği hatalı. " + ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                    data.Content = data.Content.Replace("<#text>", String.Empty).Replace("</#text>", String.Empty);
                    var question = dbContext.Question.Single(q => q.ID == data.ID);
                    question.Content = data.Content;
                    // Sorunun başlığı değişmiş ise aşşağıdaki adımları gerçekleştir.
                    if (!String.Equals(question.Title, data.Title))
                    {
                        string questionSlug = StringHelper.Slug(data.Title);
                        if (questionSlug.Length > 0)
                        {
                            if (!dbContext.Question.Any(q => q.Title.Equals(data.Title) || q.Slug.Equals(questionSlug)))
                            {
                                question.Title = data.Title;
                                question.Slug = questionSlug;
                            }
                            else
                            {
                                return Json(new { status = false, message = "Sistemde aynı başlığa kayıtlı bir soru mevcut. Sorunuza başka bir isim veriniz." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { status = false, message = "Soru için geçersiz başlık." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    // Soru Etiketlerini Ayarlayan Kod Bloğu
                    string[] tickets = data.Tickets.Split(';');

                    foreach (var item in question.Tickets.ToList())
                    {
                        if (!tickets.Contains(item.Title))
                        {
                            question.Tickets.Remove(item);
                        }
                    }

                    foreach (var item in tickets)
                    {
                        if (!question.Tickets.Any(t => t.Title.Equals(item.Trim())))
                        {
                            if (dbContext.Ticket.Any(t => t.Title.Equals(item.Trim())))
                            {
                                question.Tickets.Add(dbContext.Ticket.FirstOrDefault(t => t.Title.Equals(item.Trim())));
                            }
                            else
                            {
                                question.Tickets.Add(new Data.Entities.Ticket
                                {
                                    Title = item.Trim(),
                                    Slug = StringHelper.Slug(item.Trim())
                                });
                            }
                        }
                    }
                    try
                    {
                        dbContext.SaveChanges();

                        return Json(new { status = true, message = "Soru güncelleme işlemi başarılı.", slug = question.Slug }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception exc)
                    {
                        return Json(new { status = false, message = "Soru güncellenirken bir hata oluştu." }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json(new { status = false, message = "Gönderilen Bilgilere Ait Soru Bulunamadı. " }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = false, message = string.Join("<br/>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[Authorization(Permission = "")]
        public ActionResult Like(QustionVoteModel data)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { status = false, message = "Oy vermek için kayıtlı hesabınız ile oturum açmalısınız." }, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                int userID = int.Parse(User.Identity.Name);
                if (dbContext.Question.Any(q => q.ID == data.QuestionId))
                {

                    if (!dbContext.Question.Any(q => q.ID == data.QuestionId && q.UserID == userID))
                    {
                        // Kullanıcı Aynı soruya tekrar cevap gönderdiyse
                        if (dbContext.QuestionVoting.Any(q => q.QuestionID == data.QuestionId && q.UserID == userID))
                        {
                            var voting = dbContext.QuestionVoting.Single(q => q.QuestionID == data.QuestionId && q.UserID == userID);
                            voting.VoteType = data.VoteType;
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            var voting = dbContext.QuestionVoting.Create();
                            voting.QuestionID = data.QuestionId;
                            voting.UserID = userID;
                            voting.VoteType = data.VoteType;
                            dbContext.QuestionVoting.Add(voting);
                            dbContext.SaveChanges();
                        }
                        int likeCount = dbContext.QuestionVoting.Count(q => q.QuestionID == data.QuestionId && q.VoteType == true);
                        int disLikeCount = dbContext.QuestionVoting.Count(q => q.QuestionID == data.QuestionId && q.VoteType == false);
                        return Json(new { status = true, likeCount = likeCount, disLikeCount = disLikeCount }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false, message = "Kendi Sorunuza oy veremezsiniz." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = false, message = "Oy vermek istediğiniz soru bulunamadı" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = false, message = "Gerekli alanlar doldurulmamış." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Solved(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { status = false, message = "Kayıtlı Hesabınız İle Oturum Açmalısınız" }, JsonRequestBehavior.AllowGet);
            }

            if (dbContext.Question.Any(q => q.ID == id))
            {
                int userID = int.Parse(User.Identity.Name);
                if (dbContext.Question.Any(q => q.ID == id && q.UserID == userID))
                {
                    var question = dbContext.Question.Single(q => q.ID == id);
                    question.Status = (byte)Data.QuestionStatus.Solved;
                    question.IsSolved = true;
                    dbContext.SaveChanges();
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Sorunun Durumunu Değiştirmek İçin Soruyu Sizin Yazmanız Gerekmektedir." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = false, message = "Soru Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}