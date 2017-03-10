using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PhotoBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoBank.TagHelpers
{
    public class PhotosTagHelper : TagHelper
    {
        public ViewModels.TagsPhotoIndexViewModel photoContent { get; set; }
        public int CellsPerRow { get; set; }
        public bool ShowControls { get; set; }
        private List<Photo> photos;
        private UserManager<User> userManager;
        private IActionContextAccessor actionContextAccessor;
        private User currentUser;
        private bool isAdmin;
        public PhotosTagHelper(UserManager<User> UserManager, IActionContextAccessor ActionContextAccessor)
        {
            userManager = UserManager;
            actionContextAccessor = ActionContextAccessor;            
        }
        public override async void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";            
            photos = photoContent.Photos.ToList();            
            int cellsPerRow = 3;
            currentUser = await userManager.GetUserAsync(actionContextAccessor.ActionContext.HttpContext.User);
            isAdmin = await userManager.IsInRoleAsync(currentUser, "admin");
            for (int i = 0; i < photos.Count; i += cellsPerRow)
            {
                TagBuilder row = makeRow(i);
                output.Content.AppendHtml(row);
            }
        }

        private TagBuilder makeCellTable(Photo photo)
        {            
            TagBuilder tableCell = makeImageTableCell(photo);            
            if (ShowControls)
            {
                TagBuilder table = new TagBuilder("table");
                TagBuilder fRow = new TagBuilder("tr");
                TagBuilder sRow = new TagBuilder("tr");
                fRow.InnerHtml.AppendHtml(tableCell);
                TagBuilder tagsCell = new TagBuilder("td");
                tagsCell.MergeAttribute("valign", "top");
                tagsCell.InnerHtml.Clear();
                if (currentUser != null && (photo.UploadedByUserID == currentUser.Id || isAdmin))
                {
                    TagBuilder tagSelector = makeTagSelector();
                    tagSelector.Attributes.Clear();
                    tagSelector.MergeAttribute("onchange", string.Format("AttachTagToPhoto(this.value, {0})", photo.PhotoID));
                    tagsCell.InnerHtml.AppendHtml(tagSelector);
                    tagsCell.InnerHtml.AppendHtml(makeTagTable(photo.PhotoTags));
                    fRow.InnerHtml.AppendHtml(tagsCell);                    
                    TagBuilder deleteCell = new TagBuilder("td");
                    TagBuilder deleteAction = new TagBuilder("a");
                    deleteAction.InnerHtml.Append("Delete");
                    deleteAction.MergeAttribute("href", string.Format("/Photo/DeletePhoto?photoID={0}", photo.PhotoID));
                    deleteCell.InnerHtml.AppendHtml(deleteAction);
                    sRow.InnerHtml.AppendHtml(deleteCell);
                }
                table.InnerHtml.AppendHtml(fRow);
                if (currentUser != null)
                {
                    TagBuilder downloadCell = new TagBuilder("td");
                    TagBuilder downloadAction = new TagBuilder("a");
                    downloadAction.InnerHtml.Append("Download");
                    downloadAction.MergeAttribute("href", string.Format("/Photo/DownloadPhoto?photoID={0}", photo.PhotoID));
                    downloadCell.InnerHtml.AppendHtml(downloadAction);
                    sRow.InnerHtml.AppendHtml(downloadCell);
                }
                table.InnerHtml.AppendHtml(sRow);
                return table;
            }
            else
            {
                return tableCell;
            }
        }

        private TagBuilder makeTagTable(List<PhotoTags> photoTags)
        {
            TagBuilder tagTable = new TagBuilder("table");
            for (int j = 0; j < photoTags.Count(); ++j)
            {
                TagBuilder action = new TagBuilder("a");
                action.InnerHtml.Append(photoTags[j].Tag.TagPhrase);
                action.MergeAttribute("href", string.Format("/Tags/TagPhotos/?tagID={0}", photoTags[j].TagID));
                TagBuilder tagCell = new TagBuilder("td");
                tagCell.InnerHtml.AppendHtml(action);
                TagBuilder tagRow = new TagBuilder("tr");
                tagRow.InnerHtml.AppendHtml(tagCell);
                tagTable.InnerHtml.AppendHtml(tagRow);
            }
            return tagTable;
        }

        private TagBuilder makeTagSelector()
        {
            TagBuilder tagSelector = new TagBuilder("select");
            TagBuilder option = new TagBuilder("option");
            option.Attributes.Add("value", "-1");
            option.InnerHtml.Append("Select Tag");
            tagSelector.InnerHtml.AppendHtml(option);

            foreach (var tag in photoContent.TagSelectionList)
            {
                option = new TagBuilder("option");
                option.Attributes.Add("value", tag.Value);
                option.InnerHtml.Append(tag.Text);
                tagSelector.InnerHtml.AppendHtml(option);
            }
            tagSelector.MergeAttribute("style", "margin-top:10px");
            return tagSelector;
        }

        private TagBuilder makeImageTableCell(Photo photo)
        {
            TagBuilder imageTableCell = new TagBuilder("td");
            TagBuilder image = new TagBuilder("img");        
            var base64 = Convert.ToBase64String(photo.Data);
            image.MergeAttribute("src", string.Format("data:image/gif;base64,{0}", base64));
            image.MergeAttribute("style", "height: 100px; width: 100px; border: 4px double black; margin-right:10px; margin-left:10px");
            imageTableCell.InnerHtml.AppendHtml(image);
            return imageTableCell;
        }

        private TagBuilder makeRow(int index)
        {
            TagBuilder row = new TagBuilder("tr");
            for (int i = index; i - index < CellsPerRow && i < photos.Count; ++i)
            {
                TagBuilder cellTable = makeCellTable(photos[i]);
                if (ShowControls)
                {
                    TagBuilder td = new TagBuilder("td");
                    td.InnerHtml.AppendHtml(cellTable);
                    row.InnerHtml.AppendHtml(td);
                }
                else
                {
                    row.InnerHtml.AppendHtml(cellTable);
                }
            }
            return row;
        }
    }
}
