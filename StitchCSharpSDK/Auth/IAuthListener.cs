using System;
namespace Stitch.Auth
{
    public interface IAuthListener
    {
        void OnLogin();
        void OnLogout(string lastProvider);
    }
}
