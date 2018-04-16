﻿using System;
using System.Web.UI.WebControls;
using Umbraco.Core.Security;
using Umbraco.Core;
using Umbraco.Core.Models.Membership;


namespace Umbraco.Web.UI.Umbraco.Create
{
    public partial class User : UI.Controls.UmbracoUserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DataBind();
        }

        /// <summary>
        /// Validation to Check if Login Name Exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoginExistsCheck(object sender, ServerValidateEventArgs e)
        {
            var provider = Core.Security.MembershipProviderExtensions.GetUsersMembershipProvider();

            var user = provider.GetUser(Login.Text.Replace(" ", "").ToLower(), false);

            if (Login.Text != "" && user != null)
                e.IsValid = false;
            else
                e.IsValid = true;
        }


        /// <summary>
        /// Validation to Check if Member with email Exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EmailExistsCheck(object sender, ServerValidateEventArgs e)
        {
            var provider = Core.Security.MembershipProviderExtensions.GetUsersMembershipProvider();

            var found = provider.GetUserNameByEmail(Email.Text.ToLower());

            if (Email.Text != "" && found.IsNullOrWhiteSpace() == false && provider.RequiresUniqueEmail)
                e.IsValid = false;
            else
                e.IsValid = true;
        }

        protected void sbmt_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var formatted = string.Format("{0}|{1}", Login.Text, Email.Text);
                var returnUrl = global::umbraco.presentation.create.dialogHandler_temp.Create(
                    Request.GetItemAsString("nodeType"),
                    -1,
                    formatted);

                ClientTools
                    .ChangeContentFrameUrl(returnUrl)
                    .ChildNodeCreated()
                    .CloseModalWindow();

            }

        }

        public IUser CurrentUser
        {
            get { return Security.CurrentUser; }
        }

        protected void EmailValidator_OnServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = MembershipProviderBase.IsEmailValid(args.Value);
        }
    }
}
