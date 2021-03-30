using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using LMS.Common;
using DAL.DAO;
using DAL.EF;
using System.Web.Script.Serialization;
using System.IO;
using System.Threading.Tasks;
using Firebase.Auth;
using System.Threading;
using Firebase.Storage;

namespace LMS.Areas.Teacher.Controllers
{
    [CustomAuthorize("TEACHER")]
    public class SubjectController : Controller
    {
        // GET: Teacher/Subject
        public ActionResult Index(string id)
        {
            try
            {
                CourseDAO courseDao = new CourseDAO();
                return View(courseDao.GetCourseByID(id));
            }
            catch{
                return RedirectToAction("Error2", "Home", new { area = "" });
            }
            
        }

       [HttpPost]
        public ActionResult PostTopic(TOPIC topic)
        {
            TopicDAO topicDao = new TopicDAO();

            if (topicDao.InsertTopic(topic))
            {
                CommonFunc cf = new CommonFunc();
                cf.DelCookieFile();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }


        [HttpPost]
        public async Task<ActionResult> PostDoc(HttpPostedFileBase file, string TopicID, string DocID)
        {
            DocumentDAO docDao = new DocumentDAO();
            DOCUMENT doc = new DOCUMENT();
            doc.TOPIC_ID = TopicID;
            doc.ID = DocID;
            
              
            FileStream stream;
            if (file != null)
            {
                // string a = file.FileName;
                //string b = file.ContentType;
                string path = Path.Combine(Server.MapPath("~/Content/tempFile"), file.FileName);
                file.SaveAs(path);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
                await Task.Run(() => Upload(stream, file.FileName, path));
                doc.TITLE = file.FileName;
                doc.LINK = CommonConstants.linkFile;
                if (docDao.InsertDocument(doc))
                {
                
                    return Json(new { status = true, file = new { filename = file.FileName, link = CommonConstants.linkFile } });
                }
            }
            return Json(new { status = false });
        }


        [HttpPost]
        public async Task<ActionResult> UpdateDoc(HttpPostedFileBase file, string DocID)
        {
            DocumentDAO docDao = new DocumentDAO();
            DOCUMENT doc = new DOCUMENT();
            doc.TOPIC_ID = docDao.GetTopicIDByDoc(DocID);
            doc.ID = DocID;
            // docDao.DeleteDocument(DocID);
            string title = docDao.GetTitle(DocID);
            FileStream stream;
            if (file != null)
            {
                // string a = file.FileName;
                //string b = file.ContentType;
                string path = Path.Combine(Server.MapPath("~/Content/tempFile"), file.FileName);
                file.SaveAs(path);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
                await Task.Run(() => Delete(title));
                await Task.Run(() => Upload(stream, file.FileName, path));
                doc.TITLE = file.FileName;
                doc.LINK = CommonConstants.linkFile;
                if (docDao.UpdateDocument(doc))
                {
                    return Json(new { status = true, file = new { filename = file.FileName, link = CommonConstants.linkFile } });
                }
            }
            return Json(new { status = false });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteDoc(string id)
        {
            DocumentDAO docDao = new DocumentDAO();
            string title = docDao.GetTitle(id);
            if (docDao.DeleteDocument(id))
            {
                await Task.Run(() => Delete(title));

                return Json(new { status = true });
            }
            return Json(new { status = false });
        }

        [HttpPost]
        public ActionResult PostEvent(EVENT ev)
        {
            EventDAO eventDao = new EventDAO();
            if (eventDao.InsertEvent(ev))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult UpdateEvent(EVENT ev)
        {
            EventDAO eventDao = new EventDAO();
            if (eventDao.UpdateEvent(ev))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult DeleteEvent(string id)
        {
            EventDAO eventDao = new EventDAO();
            if (eventDao.DeleteEvent(id))
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }



        [HttpGet]
        public JsonResult GetTopic(string courseId)//string userId, string courseId, string subjectId)
        {
            CommonFunc cFunc = new CommonFunc();
            if(cFunc.GetSession() != null)
            {

                TopicDAO topicDao = new TopicDAO();
                List<TOPIC> topicEntities = new List<TOPIC>();

                topicEntities = topicDao.GetAllTopicOfTeacherCourse(cFunc.GetIdUserBySession(), cFunc.GetIdSemesterBySession(), courseId);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(topicEntities);
                return Json(new { data = json, status = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "/Home/Error", status = false });

        }

        [HttpGet]
        public JsonResult GetDocInFormAddTopicFromCookie()
        {
            if(Request.Cookies[CommonConstants.File] != null)
            {
                return Json(new { data = Request.Cookies[CommonConstants.File].Value, status =true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new {status = false }, JsonRequestBehavior.AllowGet);
        }
        
        
        [HttpPost]
        public async Task<JsonResult> DeleteTopic(string id)
        {
            TopicDAO topicDao = new TopicDAO();
            DocumentDAO docDao = new DocumentDAO();
            List<string> titles = docDao.GetTitleDocByTopicID(id);
            if (topicDao.DeleteTopic(id))
            {             
                foreach (var title in titles)
                {
                    await Delete(title);  //xoa file tren firebase
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public async Task<JsonResult> UploadFileFromFormAddTopic(HttpPostedFileBase file)
        {
            FileStream stream;
            if (file != null)
            {
                // string a = file.FileName;
                //string b = file.ContentType;
                string path = Path.Combine(Server.MapPath("~/Content/tempFile"), file.FileName);
                file.SaveAs(path);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
                await Task.Run(() => Upload(stream, file.FileName, path));

                CommonFunc commonFunc = new CommonFunc();
                if(Request.Cookies[CommonConstants.File] != null) {
                    List<Models.File> files = commonFunc.GetFileFromCookie();
                    Models.File fileModel = new Models.File(file.FileName, CommonConstants.linkFile);
                    files.Add(fileModel);
                    RootObject root = new RootObject();
                    root.File.AddRange(files);
                    files.Add(fileModel);
                    commonFunc.SetCookieFileOfTopic(root);
                }
                else
                {
                    Models.File fileModel = new Models.File(file.FileName, CommonConstants.linkFile);
                    List<Models.File> files = new List<Models.File>();
                    files.Add(fileModel);
                    RootObject root = new RootObject();
                    root.File.AddRange(files);
                    commonFunc.SetCookieFileOfTopic(root);
                }

                return Json(new { status = true, file = new { filename = file.FileName, link = CommonConstants.linkFile } });

            }
            return Json(new { status = false });
        }

        
        public async Task<JsonResult> UpdateDocFromFormAddTopic(HttpPostedFileBase file, string link)
        {
            FileStream stream;
            if (file != null)
            {
                // string a = file.FileName;
                //string b = file.ContentType;
                string path = Path.Combine(Server.MapPath("~/Content/tempFile"), file.FileName);
                file.SaveAs(path);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
                await Task.Run(() => Upload(stream, file.FileName, path));

                CommonFunc commonFunc = new CommonFunc();
                if (Request.Cookies[CommonConstants.File] != null)
                {
                    List<Models.File> files = commonFunc.GetFileFromCookie();
                    string linkT = link.Trim();
                    foreach(var itemfile in files.AsEnumerable())
                    {
                        if(String.Compare(linkT, itemfile.linkFile) == 0)
                        {
                            files.Remove(itemfile);
                            break;
                        }
                    }
                    Models.File fileModel = new Models.File(file.FileName, CommonConstants.linkFile);
                    files.Add(fileModel);
                    RootObject root = new RootObject();
                    root.File.AddRange(files);
                    files.Add(fileModel);
                    commonFunc.SetCookieFileOfTopic(root);
                }
                else
                {
                    Models.File fileModel = new Models.File(file.FileName, CommonConstants.linkFile);
                    List<Models.File> files = new List<Models.File>();
                    files.Add(fileModel);
                    RootObject root = new RootObject();
                    root.File.AddRange(files);
                    commonFunc.SetCookieFileOfTopic(root);
                }

                return Json(new { status = true, file = new { filename = file.FileName, link = CommonConstants.linkFile } });

            }
            return Json(new { status = false });
        }

        public async Task Upload(FileStream stream, string fileName, string path)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(CommonConstants.ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(CommonConstants.AuthEmail, CommonConstants.AuthPassword);
            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                    CommonConstants.Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }
                )
                .Child("file")
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);

            try
            {
                string link = await task;
                CommonConstants.linkFile = link;
                System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteFileFromFormAddTopic(string title, string link)
        {
            try
            {
                CommonFunc commonFunc = new CommonFunc();
                await Task.Run(() => Delete(title));
                List<Models.File> files = commonFunc.GetFileFromCookie();
                string linkT = link.Trim();
                foreach (var itemfile in files.AsEnumerable())
                {
                    if (String.Compare(linkT, itemfile.linkFile) == 0)
                    {
                        files.Remove(itemfile);
                        break;
                    }
                }
                return Json(new { status = true });
            }
            catch
            {
                return Json(new { status = false });
            }
           
        }

        public async Task Delete(string title)
        {
            try
            {
               
                var auth = new FirebaseAuthProvider(new FirebaseConfig(CommonConstants.ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(CommonConstants.AuthEmail, CommonConstants.AuthPassword);

                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                        CommonConstants.Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }
                    )
                    .Child("file")
                    .Child(title)
                    .DeleteAsync();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}