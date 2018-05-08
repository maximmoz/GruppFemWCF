using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GruppFemWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        List<UserInfo> GetUserInfo();

        [OperationContract]
        List<EstablishmentInfo> GetEstablishmentInfo(int? userID);
        [OperationContract]
        void DeleteUser(int userID);
        [OperationContract]
        void CreateUser(string username, string password, string firstname, string lastname, string email);
        [OperationContract]
        void UpdateUser(int userID, string username, string password, string firstname, string lastname, string email);
        [OperationContract]
        void UpdateEstablishment(int establishmentID, int rating, int userID);
        [OperationContract]
        bool LoginUser(string username, string password);
        [OperationContract]
        int GetUserID(string username, string password);
        [OperationContract]
        List<EstablishmentInfo> GetEstablishments();

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }

        
    }

    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string FName { get; set; }
        [DataMember]
        public string LName { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
    [DataContract]
    public class EstablishmentInfo
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description{ get; set; }
        [DataMember]
        public int UserRating { get; set; }
        [DataMember]
        public double Rating { get; set; }
        [DataMember]
        public List<int> UserID { get; set; }
        [DataMember]
        public List<double> URating { get; set; }
        [DataMember]
        public List<int> EstablishmentID { get; set; }
    }
}
