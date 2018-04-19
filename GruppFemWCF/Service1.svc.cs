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

        public List<EstablishmentInfo> GetEstablishmentInfo()
        {

            List<EstablishmentInfo> infoList = new List<EstablishmentInfo>();

            List<Establishment> dbEstablishmentList = db.Establishment.ToList();

            foreach (Establishment item in dbEstablishmentList)
            {

                EstablishmentInfo loopInfo = new EstablishmentInfo();
                loopInfo.ID = item.Id;
                loopInfo.Name = item.Name;
                loopInfo.Description = item.Description;
                infoList.Add(loopInfo);

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

        public void UpdateEstablishment(int establishmentID, string name, string description)
        {
            Establishment selectedEstablishment = new Establishment();
            selectedEstablishment = db.Establishment.Find(establishmentID);
            selectedEstablishment.Name = name;
            selectedEstablishment.Description = description;
            db.Entry(selectedEstablishment).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
