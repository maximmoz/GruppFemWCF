using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using GruppFemWCF.Logs;

namespace GruppFemWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {


        Grupp2.Service1Client groupTwoClient = new Grupp2.Service1Client();
        GruppFemdbEntities db = new GruppFemdbEntities();
        Log logging = new Log();

        public List<UserInfo> GetUserInfo()
        {
            try{

            
            GruppFemdbEntities db = new GruppFemdbEntities();
            List<UserInfo> infoList = new List<UserInfo>();

            List<User> dbUserList = db.User.ToList();

            foreach (User item in dbUserList)
            {

                UserInfo loopInfo = new UserInfo();
                loopInfo.ID = item.Id;
                loopInfo.Username = item.Username;
                loopInfo.Password = item.Password;
                loopInfo.FName = item.firstname;
                loopInfo.LName = item.lastname;
                loopInfo.Email = item.Email;
                infoList.Add(loopInfo);

            }
                return infoList;

            }
            catch (Exception x)
            {
                logging.CreateLogFile(x.Message);
            }

            return null;
        }

        public List<EstablishmentInfo> GetEstablishmentInfo(int? userID)
        {

            try
            {

            List<EstablishmentInfo> infoList = new List<EstablishmentInfo>();

            List<Grupp2.Business> dbEstablishmentList = groupTwoClient.GetAll();
            List<int> idList = new List<int>();
            List<double> ratingList = new List<double>();
            List<int> estList = new List<int>();
            double ratingSummary = 0;
            foreach (Grupp2.Business item in dbEstablishmentList)
            {

                EstablishmentInfo loopInfo = new EstablishmentInfo();
                loopInfo.ID = item.id;
                loopInfo.Name = item.name;
                loopInfo.Description = item.description;

                List<UserRating> dbRatingList = db.UserRating.ToList();
                var ratingCount = (from row in db.UserRating
                                   where row.EstablishmentID == item.id
                                   select row).ToList();

                foreach (var itemRating in ratingCount)
                {
                    ratingSummary += itemRating.Rating;
                }
                loopInfo.Rating = (ratingSummary / ratingCount.Count());

                if (userID != null)
                {
                    var uRating = (from row in db.UserRating
                                   where row.UserID == userID
                                   && row.EstablishmentID == item.id
                                   select row);

                    if (uRating.Count() == 1)
                    {
                        var checkRating = (from row in db.UserRating
                                           where row.UserID == userID
                                           && row.EstablishmentID == item.id
                                           select row.Rating).First();
                        idList.Add(userID.Value);
                        ratingList.Add(checkRating);
                    }
                    else
                    {
                        var hej = 0;
                        idList.Add(userID.Value);
                        ratingList.Add(hej);
                    }
                }
                loopInfo.URating = ratingList;
                loopInfo.UserID = idList;
                infoList.Add(loopInfo);
                ratingSummary = 0;
            }

            return infoList;
            }
            catch(Exception x)
            {
                logging.CreateLogFile(x.Message);
            }

            return null;
        }

        public List<EstablishmentInfo> GetEstablishments()
        {
            try
            {

            
            List<EstablishmentInfo> infoList = new List<EstablishmentInfo>();

            List<Establishment> dbEstablishmentList = db.Establishment.ToList();

            foreach (Establishment item in dbEstablishmentList)
            {

                EstablishmentInfo loopInfo = new EstablishmentInfo();
                loopInfo.ID = item.Id;
                loopInfo.Name = item.Name;
                loopInfo.Description = item.Description;
                double ratingSummary = 0;
                List<UserRating> dbRatingList = db.UserRating.ToList();
                var ratingCount = (from row in db.UserRating
                                   where row.EstablishmentID == item.Id
                                   select row).ToList();

                foreach (var itemRating in ratingCount)
                {
                    ratingSummary += itemRating.Rating;
                }
                loopInfo.Rating = (ratingSummary / ratingCount.Count());
                infoList.Add(loopInfo);
                ratingSummary = 0;
            }
            return infoList;
            }

            catch(Exception x)
            {
                logging.CreateLogFile(x.Message);
            }
            return null;
        }


        public void DeleteUser(int userID)
        {

            try
            {

            
            User selectedUser = db.User.Find(userID);

            var ratingCount = (from row in db.UserRating
                               where row.UserID == userID
                               select row).ToList();
            foreach (var itemRating in ratingCount)
            {
                UserRating selectedUserRating = db.UserRating.Find(userID, itemRating.EstablishmentID);
                db.UserRating.Remove(selectedUserRating);
            }

            
            db.User.Remove(selectedUser);
            db.SaveChanges();
            }

            catch(Exception x)
            {
                logging.CreateLogFile(x.Message);
            }
        }

        public void CreateUser(string username, string password, string firstname, string lastname, string email)
        {

            try
            {

            User createdUser = new User();
            createdUser.Username = username;
            createdUser.Password = password;
            createdUser.firstname = firstname;
            createdUser.lastname = lastname;
            createdUser.Email = email;

            db.User.Add(createdUser);
            db.SaveChanges();
            }

            catch(Exception x)
            {
                logging.CreateLogFile(x.Message);
            }
        }

        public void UpdateUser(int userID, string username, string password, string firstname, string lastname, string email)
        {

            try
            {

            
            User selectedUser = new User();
            selectedUser = db.User.Find(userID);
            selectedUser.Username = username;
            selectedUser.Password = password;
            selectedUser.firstname = firstname;
            selectedUser.lastname = lastname;
            selectedUser.Email = email;
            db.Entry(selectedUser).State = EntityState.Modified;
            db.SaveChanges();
            }

            catch(Exception x)
            {
                logging.CreateLogFile(x.Message);
            }
        }

        public void UpdateEstablishment(int establishmentID, int rating, int userID)
        {
            try
            {

            Grupp2.Business selectedEstablishment = new Grupp2.Business();
            UserRating userRating = new UserRating();
            userRating.UserID = userID;
            userRating.EstablishmentID = establishmentID;
            userRating.Rating = rating;

            var uRating = from row in db.UserRating
                          where row.UserID == userID
                          && row.EstablishmentID == establishmentID
                          select row;

            if (uRating.Count() == 1)
            {
                db.Entry(userRating).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                db.UserRating.Add(userRating);
                db.SaveChanges();
            }

            }
            catch(Exception x)
            {
                logging.CreateLogFile(x.Message);
            }
        }

        public bool LoginUser(string username, string password)
        {

            try
            {

            if (CheckUser(username, password) == true)
            {
                return true;
            }
            else
            {
                return false;

            }
            }
            catch(Exception x)
            {
                logging.CreateLogFile(x.Message);
                return false;
            }

        }

        public bool CheckUser(string username, string password)
        {


            try
            {

            var user = from row in db.User
                       where row.Username == username.ToUpper()
                       && row.Password == password
                       select row;

            if (user.Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
            }
            catch(Exception x)
            {
                logging.CreateLogFile(x.Message);
                return false;
            }
        }
        public int GetUserID(string username, string password)
        {
            try
            {

            var user = (from row in db.User
                        where row.Username == username.ToUpper()
                        && row.Password == password
                        select row.Id).First();
            return user;
            }

            catch(Exception x)
            {
                logging.CreateLogFile(x.Message);
               
            }
            return 0;
        }

        public string GetUsername(int userID)
        {
            try
            {

                var user = (from row in db.User where row.Id == userID select row.Username).First();

                return user.ToString();
            }

            catch(Exception x)
            {
                logging.CreateLogFile(x.Message);
            }
            return null;
        }
    }
}
