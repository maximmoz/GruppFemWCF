using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GruppFemWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        GruppFemdbEntities db = new GruppFemdbEntities();

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public List<UserInfo> GetUserInfo()
        {

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

        public List<EstablishmentInfo> GetEstablishmentInfo(int? userID)
        {

            List<EstablishmentInfo> infoList = new List<EstablishmentInfo>();

            List<Establishment> dbEstablishmentList = db.Establishment.ToList();
            List<int> idList = new List<int>();
            List<double> ratingList = new List<double>();
            List<int> estList = new List<int>();
            double ratingSummary = 0;
            foreach (Establishment item in dbEstablishmentList)
            {

                EstablishmentInfo loopInfo = new EstablishmentInfo();
                loopInfo.ID = item.Id;
                loopInfo.Name = item.Name;
                loopInfo.Description = item.Description;

                List<UserRating> dbRatingList = db.UserRating.ToList();
                var ratingCount = (from row in db.UserRating
                                   where row.EstablishmentID == item.Id
                                   select row).ToList();

                foreach (var itemRating in ratingCount)
                {                  
                        ratingSummary += itemRating.Rating;
                }
                loopInfo.Rating = (ratingSummary / ratingCount.Count());

                if(userID != null)
                {
                    var uRating = (from row in db.UserRating
                                  where row.UserID == userID
                                  && row.EstablishmentID == item.Id
                                  select row);
                    
                    if(uRating.Count() == 1)
                    {
                        var checkRating = (from row in db.UserRating
                                       where row.UserID == userID
                                       && row.EstablishmentID == item.Id
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

        public void DeleteUser(int userID)
        {

            User selectedUser = db.User.Find(userID);
            db.User.Remove(selectedUser);
            db.SaveChanges();
        }

        public void DeleteEstablishment(int establishmentID)
        {

            Establishment selectedEstablishment = db.Establishment.Find(establishmentID);
            db.Establishment.Remove(selectedEstablishment);
            db.SaveChanges();
        }

        public void CreateUser(string username, string password, string firstname, string lastname, string email)
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

        public void CreateEstablishment(string name, string description)
        {
            Establishment createdEstablishment = new Establishment();
            createdEstablishment.Name = name;
            createdEstablishment.Description = description;
            
            db.Establishment.Add(createdEstablishment);
            db.SaveChanges();
        }

        public void UpdateUser(int userID, string username, string password, string firstname, string lastname, string email)
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

        public void UpdateEstablishment(int establishmentID, string name, string description, int rating, int userID)
        {

            Establishment selectedEstablishment = new Establishment();
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
            

            selectedEstablishment = db.Establishment.Find(establishmentID);
            selectedEstablishment.Name = name;
            selectedEstablishment.Description = description;
            db.Entry(selectedEstablishment).State = EntityState.Modified;
            db.SaveChanges();
        }

        public bool LoginUser(string username, string password)
        {
            

            if(CheckUser(username, password) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckUser(string username, string password)
        {

            var user = from row in db.User
                       where row.Username == username.ToUpper()
                       && row.Password == password select row;

            if (user.Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int GetUserID(string username, string password)
        {
            var user = (from row in db.User
                      where row.Username == username.ToUpper()
                      && row.Password == password
                      select row.Id).First();
            return user;
        }
    }
}
