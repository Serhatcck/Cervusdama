using CervusDama.Authorization;
using CervusDama.Data.Model.AnswerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
    public class AnswerController : Base.BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Insert(string slug, int questionId)
        {
            AnswerInsertModel model = new AnswerInsertModel();
            model.QuestionId = questionId;
            model.Slug = slug;
            return View(model);
        }

        /*5 dakika içinde sadece bir cevap kontrolü*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] //xss açığı kontrol.
        [Authorization(Permission = "")]
        public ActionResult Insert(AnswerInsertModel answer)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Question.Any(q => q.ID == answer.QuestionId && q.Slug.Equals(answer.Slug)))
                {
                    int userID = int.Parse(User.Identity.Name);
                    if (dbContext.Answer.Any(a => a.QuestionID == answer.QuestionId && a.UserID == userID))
                    {
                        /*DateTime lastQuestionCommentTime = dbContext.Answer.OrderBy(o => o.CreateAt).
                            FirstOrDefault(a => a.QuestionID == answer.QuestionId && a.UserID == userID)
                            CreateAt;*/
                        DateTime lastQuestionCommentTime = dbContext.Answer.Where(a => a.QuestionID == answer.QuestionId && a.UserID == userID).OrderByDescending(o => o.CreateAt)
                            .FirstOrDefault().CreateAt;
                        if (DateTime.Now < lastQuestionCommentTime.AddMinutes(5))
                        {
                            TempData["message"] = "5 Dakika İçerisinde Sadece Bir Cevap Ekleyebilirsiniz";
                            return RedirectToRoute("QuestionDetail", new { slug = answer.Slug });
                        }
                    }
                    var questionAnswer = dbContext.Answer.Create();
                    questionAnswer.UserID = userID;
                    questionAnswer.QuestionID = answer.QuestionId;
                    questionAnswer.Content = answer.Content;
                    questionAnswer.CreateAt = DateTime.Now;
                    questionAnswer.Status = 1;
                    try
                    {
                        dbContext.Answer.Add(questionAnswer);
                        dbContext.SaveChanges();
                        TempData["message"] = "Cevap Sisteme Başarılı Bir Şekilde Eklendi";
                        return RedirectToRoute("QuestionDetail", new { slug = answer.Slug });
                    }
                    catch
                    {
                        TempData["message"] = "Cevap Sisteme Kayıt Edilirken Bir Hata İle Karşılaşıldı";
                        return RedirectToRoute("QuestionDetail", new { slug = answer.Slug });
                    }
                }
                else
                {
                    TempData["message"] = "Cevap Eklemek İstediğiniz Soru Sistemde Bulunamadı";
                    return RedirectToRoute("QuestionDetail", new { slug = answer.Slug });
                }

            }
            else
            {
                TempData["message"] = string.Join("<br/>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return RedirectToRoute("QuestionDetail", new { slug = answer.Slug });
            }
            //return Json(new { status = false, message = "Gerekli alanları doldurmalısınız." }, JsonRequestBehavior.AllowGet);


            //return RedirectToRoute("QuestionDetail", new { slug = answer.Slug });
        }

        [HttpPost]
        public ActionResult Like(AnswerVoteModel data)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { status = false, message = "Oy vermek için kayıtlı hesabınız ile oturum açmalısınız." }, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                int userID = int.Parse(User.Identity.Name);
                if (dbContext.Answer.Any(a => a.ID == data.AnswerID))
                {
                    if (!dbContext.Answer.Any(a => a.ID == data.AnswerID && a.UserID == userID))
                    {
                        //Kullanıcı aynı cevaba tekrar istek gönderirse
                        if (dbContext.AnswerVoting.Any(a => a.AnswerID == data.AnswerID && a.UserID == userID))
                        {
                            var voting = dbContext.AnswerVoting.Single(a => a.AnswerID == data.AnswerID && a.UserID == userID);
                            voting.VoteType = data.VoteType;
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            var voting = dbContext.AnswerVoting.Create();
                            voting.AnswerID = data.AnswerID;
                            voting.UserID = userID;
                            voting.VoteType = data.VoteType;
                            dbContext.AnswerVoting.Add(voting);
                            dbContext.SaveChanges();
                        }
                        int likeCount = dbContext.AnswerVoting.Count(a => a.AnswerID == data.AnswerID && a.VoteType == true);
                        int dislikeCount = dbContext.AnswerVoting.Count(a => a.AnswerID == data.AnswerID && a.VoteType == false);
                        return Json(new { status = true, likeCount = likeCount, disLikeCount = dislikeCount }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false, message = "Kendi Cevabınıza oy veremezsiniz." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = false, message = "Oy vermek istediğiniz cevap bulunamadı" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = false, message = "Gerekli alanlar doldurulmamış." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Solved(AnswerSolvedModel data)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { status = false, message = "Oy vermek için kayıtlı hesabınız ile oturum açmalısınız." }, JsonRequestBehavior.AllowGet);
            }
            if (ModelState.IsValid)
            {
                int userID = int.Parse(User.Identity.Name);
                if (dbContext.Question.Any(q => q.ID == data.QuestionID))
                {
                    if (dbContext.Question.Any(q => q.ID == data.QuestionID && q.UserID == userID))
                    {
                        if (dbContext.Answer.Any(a => a.ID == data.AnswerID && a.QuestionID == data.QuestionID))
                        {
                            var answer = dbContext.Answer.Single(a => a.ID == data.AnswerID);
                            answer.isSolved = true;
                            var question = dbContext.Question.Single(q => q.ID == data.QuestionID);
                            question.IsSolved = true;
                            dbContext.SaveChanges();
                            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { status = false, message = "Soruya ait cevap bulunamadı" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { status = false, message = "Cevabın çözüldü bilgisini ancak sorunun sahibi verebilir." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = false, message = "Soru bulunamadı." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = false, message = "Gerekli alanlar doldurulmamış." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [Authorization(Permission ="")]
        public JsonResult Delete(int id)
        {
            if (dbContext.Answer.Any(a => a.ID == id))
            {
                int userID = int.Parse(User.Identity.Name);
                if (dbContext.Answer.Any(a => a.ID == id && a.UserID == userID))
                {
                    var answer = dbContext.Answer.Single(a => a.ID == id);
                    foreach(var voting in dbContext.AnswerVoting.Where(v=>v.AnswerID == answer.ID))
                    {
                        dbContext.AnswerVoting.Remove(voting);
                    }
                    dbContext.Answer.Remove(answer);
                    try
                    {
                        dbContext.SaveChanges();
                        return Json(new { status = true, message = "Cevap Başarı İle Silindi" });
                    }
                    catch (Exception exc)
                    {
                        return Json(new { status = true, message = "Cevap Silinirken Bir Hata Oluştu" });
                    }
                }
                else
                {
                    return Json(new { status = false, message = "Sadece Kendi Cevabınızı Silebilirsiniz" });
                }
            }
            else
            {
                return Json(new { status = false, message = "Silmek İstenilen Cevap Bulunamadı" });
            }
        }

        public ActionResult Edit()
        {
            AnswerEditModel model = new AnswerEditModel();
            return View(model);
        }

        [HttpPost]
        [Authorization(Permission ="")]
        public ActionResult Edit(AnswerEditModel data)
        {
            if(dbContext.Answer.Any(a=> a.ID == data.ID))
            {
                int userID = int.Parse(User.Identity.Name);
                if(dbContext.Answer.Any(a=> a.ID == data.ID && a.UserID == userID))
                {
                    var answer = dbContext.Answer.Single(a => a.ID == data.ID);
                    answer.Content = data.Content;
                    try
                    {
                        dbContext.SaveChanges();
                        return Json(new { status = true, message = "Cevap Başarılı Bir Şekilde Güncellendi" });
                    }catch(Exception exc)
                    {
                        return Json(new { status = false, message = "Cevap Güncellenirken Bir Hata Oluştu" });
                    }
                }else
                {
                    return Json(new { status = false, message = "Size Ait Olmayan Cevabı Güncelleyemezsiniz" });
                }
            }else
            {
                return Json(new { status = false, message = "Cevap Bulunamadı" });
            }
        }
    }
}