using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VRWebServiceLibrary.Model;

namespace VRWebServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRentalService" in both code and config file together.
    [ServiceContract]
    public interface IRentalService
    {
        [OperationContract]
        IEnumerable<Rental> GetRentals();

        [OperationContract]
        Rental GetRental(int Id);

        [OperationContract]
        bool PutRental(int Id, Rental rental);

        [OperationContract]
        int PostRental(Rental rental);

        [OperationContract]
        bool DeleteRental(int Id);
    }
}
