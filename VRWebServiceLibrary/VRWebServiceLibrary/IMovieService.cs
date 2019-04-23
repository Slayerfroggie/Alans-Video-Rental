using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Http;
using VRWebServiceLibrary.Model;

namespace VRWebServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMovieService" in both code and config file together.
    [ServiceContract]
    public interface IMovieService
    {
        [OperationContract]
        IEnumerable<Movie> GetMovies();

        [OperationContract]
        Movie GetMovie(int Id);

        [OperationContract]
        bool PutMovie(int Id, Movie movie);

        [OperationContract]
        int PostMovie(Movie movie);

        [OperationContract]
        bool DeleteMovie(int Id);
    }

}
