using ChatService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ChatService.WCFService
{
    [ServiceContract(CallbackContract = typeof(IServiceCallback), SessionMode = SessionMode.Required)]
    public interface IService
    {
        [OperationContract(IsOneWay = true)]
        void LoginUser(string userName);

        [OperationContract(IsOneWay = true)]
        void SendMessage(Message message);

        [OperationContract(IsOneWay = true)]
        void CreateRoom(string name);

        [OperationContract(IsOneWay = true)]
        void EnterRoom(User user, Room room);

        [OperationContract(IsOneWay = true)]
        void LeaveRoom(User user, Room room);

        [OperationContract(IsOneWay = true)]
        void Logout(User user);

    }
}
