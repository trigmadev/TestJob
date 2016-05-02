using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Graphics;
using Jornalero.Core;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace Jornalero.droid
{
    public class AddVehicleFragment : Fragment
    {
        #region Variables

        View view;
        int VehicleColorId;
        int VehicleType;
        int AreaId;
       
        EditText vehiclenumber;
        Spinner spinner;
        GridView gridviewVehicleType;
        CreateAlert DatefromAlert;

        #endregion

        public AddVehicleFragment()
        {
            this.RetainInstance = true;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (view == null)
            {
                view = inflater.Inflate(Resource.Layout.AddVehicle, container, false);
            }

            vehiclenumber = view.FindViewById<EditText>(Resource.Id.vNumber);

            var adapter = ArrayAdapter.CreateFromResource(this.Activity, Resource.Array.planets_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner = view.FindViewById<Spinner>(Resource.Id.spinner);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            spinner.Adapter = adapter;

            DatefromAlert = (CreateAlert)Arguments.GetParcelable("CreateAlert");


            var gridviewColor = view.FindViewById<GridView>(Resource.Id.gridviewColor);
            gridviewColor.Adapter = new ImageAdapter(this.Activity,false);
            gridviewColor.ItemClick += GridviewColor_ItemClick;
            //gridviewColor.Post(() => {
            //    gridviewColor.RequestFocusFromTouch();
            //    gridviewColor.SetSelection(DatefromAlert.VehicleColor);
            //});

            gridviewVehicleType = view.FindViewById<GridView>(Resource.Id.gridview);
            gridviewVehicleType.Adapter = new ImageAdapter(this.Activity, true);
            gridviewVehicleType.ItemClick += Gridview_ItemClick;
            gridviewVehicleType.Post(() => {
                gridviewVehicleType.RequestFocusFromTouch();
                gridviewVehicleType.SetSelection(DatefromAlert.VehicleTypeId);
                var v = gridviewVehicleType.SelectedItemPosition;

                gridviewColor.RequestFocusFromTouch();
                gridviewColor.SetSelection(DatefromAlert.VehicleColor);
            });

            var btnShare = view.FindViewById<Button>(Resource.Id.avbtnShare);
            btnShare.Click += BtnShare_Click;

            var btnBack = view.FindViewById<Button>(Resource.Id.avbtnBack);
            btnBack.Click += BtnBack_Click;

            spinner.SetSelection(DatefromAlert.LicenseAreaId);
            if (DatefromAlert.LicenseNumber != null)
            {
                vehiclenumber.Text = SecurityClass.DecryptAes(DatefromAlert.LicenseNumber);
            }
            return view;
        }

        #region Events

        private void GridviewColor_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            VehicleColorId = e.Position;
        }

        private void Gridview_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            VehicleType = e.Position;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            DatefromAlert.VehicleColor = VehicleColorId;
            DatefromAlert.VehicleTypeId = VehicleType;
            DatefromAlert.LicenseAreaId = AreaId;
            DatefromAlert.LicenseNumber = SecurityClass.EncryptAes(vehiclenumber.Text.Trim());
            FragmentManager fm = this.Activity.SupportFragmentManager;
            fm.PopBackStack(null, FragmentManager.PopBackStackInclusive);
        }

        private void BtnShare_Click(object sender, EventArgs e)
        {
            if (vehiclenumber.Text == null || vehiclenumber.Text.Replace("\n", "").Length == 0)
            {
                vehiclenumber.Text = "";
                vehiclenumber.Hint = Core.Constants.BlankMsg;
                vehiclenumber.SetHintTextColor(Color.ParseColor(Core.Constants.error_color));
                vehiclenumber.SetBackgroundResource(Resource.Drawable.error_bg);
                return;
            }
            
            CreateAlert objUser = new CreateAlert();
            objUser.Description = DatefromAlert.Description;
            objUser.Image = DatefromAlert.Image;
            objUser.IsImageChange = true;
            objUser.IsVechicle = true;
            objUser.Latitude = 30.7654;
            objUser.Longitude = 71.7654;
            objUser.LicenseAreaId = AreaId;
            objUser.LicenseNumber = SecurityClass.EncryptAes(vehiclenumber.Text.Trim());
            objUser.UserLaborId = Core.Constants.LaborId;
            objUser.VehicleColor = VehicleColorId;
            objUser.VehicleTypeId = VehicleType;

            JornaleroResponse<int> objResp = new JornaleroResponse<int>();
            ServiceHandler.PostData<JornaleroResponse<int>, CreateAlert>(Jornalero.Core.Constants.CreateAlert, HttpMethod.Post,
                objUser).ContinueWith((completed) =>
                {
                    if (!completed.IsFaulted)
                    {
                        objResp = completed.Result;
                        if (objResp != null)
                        {
                            if (objResp.status_code == (int)HttpStatusCode.OK)
                            {
                                Toast.MakeText(Context, "Alert is created successfully", ToastLength.Long).Show();
                            }
                            else
                                Toast.MakeText(Context, objResp.message, ToastLength.Long).Show();
                        }
                        else
                            Toast.MakeText(Context, Core.Constants.ErrorMessageEnglish, ToastLength.Long).Show();
                    }
                    else
                        Toast.MakeText(Context, Core.Constants.ErrorMessageEnglish, ToastLength.Long).Show();
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            AreaId = e.Position;
        }

        #endregion

    }

    public class ImageAdapter : BaseAdapter
    {
        Context context;

        bool flag;

        public ImageAdapter(Context c,bool fl)
        {
            context = c;
            flag = fl;
        }

        public override int Count
        {
            get
            {
                if (flag)
                {
                    return thumbIds.Length;
                }
                return colorIds.Length;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView imageView;

            if (convertView == null)
            {  
                imageView = new ImageView(context);
                imageView.SetPadding(10,10,10,10);
            }
            else
            {
                imageView = (ImageView)convertView;
            }

            if (flag)
            {
                imageView.SetImageResource(thumbIds[position]);
            }
            else
            {
                imageView.SetImageResource(colorIds[position]);
            }
            return imageView;
        }

        int[] thumbIds = { Resource.Drawable.img1, Resource.Drawable.img2, Resource.Drawable.img3, Resource.Drawable.img4, Resource.Drawable.img5, Resource.Drawable.img6 };
        int[] colorIds = { Resource.Drawable.white, Resource.Drawable.black, Resource.Drawable.grey, Resource.Drawable.red, Resource.Drawable.blue, Resource.Drawable.dark_grey, Resource.Drawable.brown, Resource.Drawable.green, Resource.Drawable.yellow};
    }
}

