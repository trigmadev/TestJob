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
using Android.Provider;
using Java.IO;
using Android.Content.PM;
using PInvoke;
using Jornalero.Core;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.App;

namespace Jornalero.droid
{
    public class CreateAlertFragment : Fragment
    {
        #region Variables
        
        PicturePickType flag;

        View view;

        string imageuri;
        ImageView imageView;
        EditText description;

        LinearLayout llVehicle;
        LinearLayout llButton;

        Java.IO.File file;
        Java.IO.File directory;

        Button Upload;
        Button TakePic;
        Button btnShare;
        LinearLayout addnewonebtn;
        LinearLayout deletebtn;
        Button btnUpload;
        Dialog dialogue;

        CreateAlert createAlert;

        #endregion

        #region Create View, Initialize UI Controls

        public CreateAlertFragment()
        {
            this.RetainInstance = true;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            createAlert = new CreateAlert();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (view == null)
            {
                view = inflater.Inflate(Resource.Layout.CreateAlert, container, false);
            }
            InitializeComponents();

            return view;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            llVehicle.Click -= LlVehicle_Click;
            btnUpload.Click -= BtnUpload_Click;
            Upload.Click -= Upload_Click;
            TakePic.Click -= TakePic_Click;
            btnShare.Click -= BtnShare_Click;
            addnewonebtn.Click -= Addnewonebtn_Click;
            deletebtn.Click -= Deletebtn_Click;
        }

        private void InitializeComponents()
        {
            dialogue = new Dialog(Context);
            dialogue.RequestWindowFeature(1);
            dialogue.SetContentView(Resource.Layout.UploadButtonsDialogue);
            dialogue.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

            llVehicle = view.FindViewById<LinearLayout>(Resource.Id.llVehicle);
            Upload = dialogue.FindViewById<Button>(Resource.Id.btnUploadImage);
            TakePic = dialogue.FindViewById<Button>(Resource.Id.btnTakeImage);
            btnShare = view.FindViewById<Button>(Resource.Id.btnShare);
            addnewonebtn = view.FindViewById<LinearLayout>(Resource.Id.llAddButton);
            deletebtn = view.FindViewById<LinearLayout>(Resource.Id.rltvDeleteButton);

            imageView = view.FindViewById<ImageView>(Resource.Id.imgUpload);
            description = view.FindViewById<EditText>(Resource.Id.caTypeHere);
            llButton = view.FindViewById<LinearLayout>(Resource.Id.llButton);
            btnUpload = view.FindViewById<Button>(Resource.Id.btnUpload);

            llVehicle.Click += LlVehicle_Click;
            btnUpload.Click += BtnUpload_Click;
            Upload.Click += Upload_Click;
            TakePic.Click += TakePic_Click;
            btnShare.Click += BtnShare_Click;
            addnewonebtn.Click += Addnewonebtn_Click;
            deletebtn.Click += Deletebtn_Click;
        }

        #endregion

        #region Events

        private void Deletebtn_Click(object sender, EventArgs e)
        {
            imageView.SetImageResource(Resource.Drawable.camara);
            imageuri = null;
            llButton.Visibility = ViewStates.Gone;
            btnUpload.Visibility = ViewStates.Visible;
        }

        private void Addnewonebtn_Click(object sender, EventArgs e)
        {
            dialogue.Show();
        }

        private void TakePic_Click(object sender, EventArgs e)
        {
            flag = PicturePickType.TakeNew;
            dialogue.Dismiss();
            directory = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "Jornalero");
            if (!directory.Exists())
            {
                directory.Mkdirs();
            }
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            file = new File(directory, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(file));
            StartActivityForResult(intent, 100);
        }

        private void Upload_Click(object sender, EventArgs e)
        {
            flag = PicturePickType.Upload;
            dialogue.Dismiss();
            Intent Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), 0);
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            dialogue.Show();
        }

        private void LlVehicle_Click(object sender, EventArgs e)
        {
            Fragment fmt = new AddVehicleFragment();
            Android.Support.V4.App.FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();
            Bundle args = new Bundle();
            createAlert.Description = SecurityClass.EncryptAes(description.Text.Trim());
            createAlert.Image = imageuri;
            args.PutParcelable("CreateAlert", createAlert);
            fmt.Arguments = args;
            fragmentTransaction.Replace(Resource.Id.content_frame, fmt);
            fragmentTransaction.AddToBackStack(null);
            fragmentTransaction.Commit();
        }

        private void BtnShare_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(description.Text))
            {
                description.Hint = Core.Constants.BlankMsg;
                description.SetHintTextColor(Color.ParseColor(Core.Constants.error_color));
                description.SetBackgroundResource(Resource.Drawable.error_bg);
                return;
            }
            else
            {
                createAlert.Description = SecurityClass.EncryptAes(description.Text);
                createAlert.Latitude = 30.7654;
                createAlert.Longitude = 71.7654;
                if (createAlert.VehicleTypeId >= 0)
                    createAlert.IsVechicle = true;
                createAlert.IsImageChange = true;
                createAlert.UserLaborId = Jornalero.Core.Constants.LaborId;
                createAlert.Image = imageuri;
                JornaleroResponse<int> objResp = new JornaleroResponse<int>();
                ServiceHandler.PostData<JornaleroResponse<int>, CreateAlert>(Jornalero.Core.Constants.CreateAlert, HttpMethod.Post,
                    createAlert).ContinueWith((completed) =>
                    {
                        if (!completed.IsFaulted)
                        {
                            objResp = completed.Result;
                            if (objResp != null)
                            {
                                if (objResp.status_code == (int)HttpStatusCode.OK)
                                {
                                    Toast.MakeText(this.Context, "Alert is created successfully", ToastLength.Long).Show();
                                }
                                else
                                    Toast.MakeText(this.Context, objResp.message, ToastLength.Long).Show();
                            }
                            else
                                Toast.MakeText(this.Context, Core.Constants.ErrorMessageEnglish, ToastLength.Long).Show();
                        }
                        else
                            Toast.MakeText(this.Context, Core.Constants.ErrorMessageEnglish, ToastLength.Long).Show();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        #endregion

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (resultCode == 0)
            {
                return;
            }

            base.OnActivityResult(requestCode, resultCode, data);

            if (flag == PicturePickType.Upload)
            {
                if (data != null)
                {
                    Android.Net.Uri uri = data.Data;
                    imageuri = uri.Path;
                    imageView.SetImageURI(uri);
                }
            }
            else
            {
                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Android.Net.Uri contentUri = Android.Net.Uri.FromFile(file);
                mediaScanIntent.SetData(contentUri);
                Activity.SendBroadcast(mediaScanIntent);
                imageuri = contentUri.Path;
                using (Bitmap bitmap = BitmapFactory.DecodeFile(imageuri))
                {
                    imageView.SetImageBitmap(bitmap);
                }
            }

            llButton.Visibility = ViewStates.Visible;
            btnUpload.Visibility = ViewStates.Gone;

            imageView.DrawingCacheEnabled = true;
            imageView.BuildDrawingCache();
            Bitmap img = imageView.GetDrawingCache(true);

            var stream = new System.IO.MemoryStream();
            img.Compress(Bitmap.CompressFormat.Png, 100, stream);
            var bytes = stream.ToArray();
            imageuri = Convert.ToBase64String(bytes);
        }
    }

    public enum PicturePickType
    {
        Upload,
        TakeNew
    }
}