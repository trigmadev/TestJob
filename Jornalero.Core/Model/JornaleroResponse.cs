using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Runtime;

namespace Jornalero.Core
{
    public class JornaleroResponse<T>
    {
        public int status_code { get; set; }

        public T data { get; set; }

        public string message { get; set; }
    }

    public class JornaleroResponseList<T>
    {
        public int status_code { get; set; }

        public T data { get; set; }

        public string message { get; set; }

        public int TotalCount { get; set; }
    }

    public class LaborUserLogin
    {
        public int UserId { get; set; }

        public byte[] LoginId { get; set; }

        public byte[] Password { get; set; }
    }

    public class LoginResponse
    {
        public int UserId { get; set; }

        public byte[] FirstName { get; set; }

        public byte[] LastName { get; set; }

        public byte[] Email { get; set; }

        public byte[] Password { get; set; }

        public string Age { get; set; }

        public int Gender { get; set; }

        public int ProfileCompleteness { get; set; }

        public int OriginId { get; set; }

        public string OriginName { get; set; }

        public int OccupationId { get; set; }

        public string OccupationName { get; set; }
    }

    public class RegisterLabor
    {
        public string WorkCenterCode { get; set; }

        public int ProfessionId { get; set; }

        public int BirthPlaceId { get; set; }

        public byte[] FirstName { get; set; }

        public byte[] LastName { get; set; }

        public byte[] Email { get; set; }

        public byte[] Password { get; set; }

        public string Age { get; set; }

        public int Gender { get; set; }

        public int ProfileCompleteness { get; set; }

        public bool IsAgreed { get; set; }

        public bool IsActive { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedDate { get; set; }

        public string DeviceToken { get; set; }

        public string DeviceType { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }

    public class WorkCenter
    {
        public int WorkCenterId { get; set; }

        public string Name { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string Address { get; set; }
    }

    public class BirthPlace
    {
        public int BirthPlaceId { get; set; }

        public string Name { get; set; }
    }

    public class Profession
    {
        public int ProfessionId { get; set; }

        public string Name { get; set; }
    }

    public class Area
    {
        public int AreaId { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }

        public string Message { get; set; }
    }

    public class LaborProfile
    {
        public int UserId { get; set; }

        public byte[] Email { get; set; }

        public byte[] Password { get; set; }

        public int WorkCenter { get; set; }
    }

    public class PersonalInfo
    {
        public int UserId { get; set; }

        public byte[] FirstName { get; set; }

        public byte[] LastName { get; set; }

        public int Gender { get; set; }

        public string Age { get; set; }

        public int BirthPlace { get; set; }

        public int Profession { get; set; }
    }

    public enum UserRole : int
    {
        SuperAdmin,
        CenterAdmin,
        Admin,
        LaborUser,
    }

    public class ForgotPassword
    {
        public string VerificationCode { get; set; }

        public int UserId { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }

        public byte[] Email { get; set; }
    }

    public class CreateAlert : Java.Lang.Object,IParcelable
    {
        public CreateAlert()
        {
            this.VehicleTypeId = -1;
            this.VehicleColor = -1;
        }

        public int AlertId { get; set; }

        public int UserLaborId { get; set; }

        public bool IsVechicle { get; set; }

        public int VehicleInfoId { get; set; }

        public int LicenseAreaId { get; set; }

        public byte[] Description { get; set; }

        public string Image { get; set; }

        public byte[] LicenseNumber { get; set; }

        public int VehicleTypeId { get; set; }

        public int VehicleColor { get; set; }

        public string VehicleImage { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsImageChange { get; set; }

        public int DescribeContents()
        {
            return 0;
        }

        public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
        {
            
        }
    }

}