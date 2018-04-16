﻿using System.Collections.Generic;
using System.IO;
using System;
using System.Net.Http.Formatting;
using Umbraco.Core;
using Umbraco.Core.IO;
using Umbraco.Web.Composing;
using Umbraco.Core.Services;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi.Filters;
using Umbraco.Web._Legacy.Actions;
using Constants = Umbraco.Core.Constants;

namespace Umbraco.Web.Trees
{
    [UmbracoTreeAuthorize(Constants.Trees.Xslt)]
    [Tree(Constants.Applications.Settings, Constants.Trees.Xslt, "XSLT Files", "icon-folder", "icon-folder", sortOrder: 2)]
    [PluginController("UmbracoTrees")]
    [CoreTree]
    public class XsltTreeController : FileSystemTreeController
    {
        protected override void OnRenderFileNode(ref TreeNode treeNode)
        {
            ////TODO: This is all hacky ... don't have time to convert the tree, views and dialogs over properly so we'll keep using the legacy views
            treeNode.AssignLegacyJsCallback("javascript:UmbClientMgr.contentFrame('developer/xslt/editXslt.aspx?file=" + treeNode.Id + "');");
        }

        protected override void OnRenderFolderNode(ref TreeNode treeNode)
        {
            //TODO: This is all hacky ... don't have time to convert the tree, views and dialogs over properly so we'll keep using the legacy views
            treeNode.AssignLegacyJsCallback("javascript:void(0);");
        }

        protected override void GetMenuForFile(MenuItemCollection menu, string path, string id, FormDataCollection queryStrings)
        {
            //TODO: This is all hacky ... don't have time to convert the tree, views and dialogs over properly so we'll keep using the legacy dialogs
            var menuItem = menu.Items.Add(ActionDelete.Instance, Services.TextService.Localize(string.Format("actions/{0}", ActionDelete.Instance.Alias)));
            var legacyConfirmView = LegacyTreeDataConverter.GetLegacyConfirmView(ActionDelete.Instance);
            if (legacyConfirmView == false)
                throw new InvalidOperationException("Could not resolve the confirmation view for the legacy action " + ActionDelete.Instance.Alias);
            menuItem.LaunchDialogView(
                legacyConfirmView.Result,
                Services.TextService.Localize("general/delete"));
        }

        protected override void GetMenuForRootNode(MenuItemCollection menu, FormDataCollection queryStrings)
        {
            //set the default to create
            menu.DefaultMenuAlias = ActionNew.Instance.Alias;

            // root actions
            menu.Items.Add<ActionNew>(Services.TextService.Localize(string.Format("actions/{0}", ActionNew.Instance.Alias)))
                .ConvertLegacyMenuItem(null, Constants.Trees.Xslt, queryStrings.GetValue<string>("application"));

            menu.Items.Add<RefreshNode, ActionRefresh>(Services.TextService.Localize("actions", ActionRefresh.Instance.Alias), true);
        }

        protected override IFileSystem FileSystem => Current.FileSystems.XsltFileSystem; // fixme inject

        private static readonly string[] ExtensionsStatic = { "xslt" };

        protected override string[] Extensions => ExtensionsStatic;

        protected override string FileIcon => "icon-code";

        protected override bool EnableCreateOnFolder => true;
    }
}
