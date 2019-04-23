using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VRWebServiceLibrary.Model;

namespace VRWebServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRentalItemService" in both code and config file together.
    [ServiceContract]
    public interface IRentalItemService
    {
        [OperationContract]
        IEnumerable<RentalItem> GetRentalItems();

        [OperationContract]
        RentalItem GetRentalItem(int Id);

        [OperationContract]
        IEnumerable<RentalItem> GetRentalItemByRentalId(int Id);

        [OperationContract]
        bool PutRentalItem(int Id, RentalItem rentalItem);

        [OperationContract]
        int PostRentalItem(RentalItem rentalItem);

        [OperationContract]
        int DeleteRentalItem(int Id);
    }
}
