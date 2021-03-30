using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DAL.DAO;
using DAL.EF;
using DAL.StudentView;
using Firebase.Auth;
using Firebase.Storage;
using LMS.Common;
using LMS.Models;

namespace LMS.Areas.Student.Controllers
{
    [CustomAuthorize("STUDENT")]
    public class SubjectController : Controller
    {

        public ActionResult GetTopicStudent(string course_id)
        {
            CommonFunc cFunc = new CommonFunc();
            TopicDAO tDAO = new TopicDAO();
            CourseDAO cDAO = new CourseDAO();
            COURSE course = cDAO.GetCourseByID(course_id);
            List<TOPIC> topics = tDAO.GetAllTopicOfStudentCourse(cFunc.GetIdUserBySession(), course_id);
            CourseDetailsView model = new CourseDetailsView();
            model.course = course;
            model.topics = topics;
            return View(model);

        }
        public ActionResult GetSubjectAssessments(string course_id) 
        { 
            CommonFunc cFunc = new CommonFunc();
            SubmitDAO subDAO = new SubmitDAO();
            COURSE course = subDAO.GetCourseWithEventAndSubmmit(cFunc.GetIdUserBySession(), course_id);
            return View(course);

        }
        public ActionResult Submit(string event_id)
        {
            EventDAO eDao = new EventDAO();
            EVENT ev = eDao.GetEventByID(event_id);
            return View(ev);
        }
        
        [HttpPost]
        public async Task<JsonResult> SubmitFile(HttpPostedFileBase file, string Event_ID, string Submit_ID)
        {
            FileStream stream;
            if (file != null)
            {
                string path = Path.Combine(Server.MapPath("~/Content/tempFile"), file.FileName);
                file.SaveAs(path);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
                await Task.Run(() => Upload(stream, file.FileName, path));
                SUBMIT sub = new SUBMIT();
                sub.ID = Submit_ID;
                sub.EVENT_ID = Event_ID;
                sub.USER_ID = (Session[CommonConstants.SESSION] as Session).id_user;
                sub.LINK = CommonConstants.linkFile;
                sub.TIME = DateTime.Now;
                SubmitDAO submitDAO = new SubmitDAO();
                if (submitDAO.InsertSubmit(sub))
                {
                    return Json(new { status = true, file = new { filename = file.FileName, link = CommonConstants.linkFile } });
                }
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
    }
}