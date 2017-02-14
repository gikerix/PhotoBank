using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using PhotoBank.Models;

namespace PhotoBank.TagHelpers
{
    public class PhotosTagHelper : TagHelper
    {
        public ViewModels.TagsPhotoIndexViewModel photoContent { get; set; }       
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";            
            List<Photo> photos = photoContent.Photos.ToList();
            TagBuilder table = new TagBuilder("table");
            for (int i = 0; i < photos.Count; ++i)
            {
                TagBuilder image = new TagBuilder("img");
                image.InnerHtml.Clear();
                image.Attributes.Clear();
                TagBuilder imageCell = new TagBuilder("td");                
                var photo = photos[i];
                var base64 = Convert.ToBase64String(photo.Data);
                image.MergeAttribute("src", string.Format("data:image/gif;base64,{0}", base64));
                image.MergeAttribute("style", "height: 100px; width: 100px; border: 4px double black");
                imageCell.InnerHtml.AppendHtml(image);
                TagBuilder row = new TagBuilder("tr");
                row.InnerHtml.AppendHtml(imageCell);
                TagBuilder tagsCell = new TagBuilder("td");
                tagsCell.MergeAttribute("valign", "top");
                tagsCell.InnerHtml.Clear();
                TagBuilder tagSelector = CreateTagSelector();
                tagSelector.Attributes.Clear();
                tagSelector.MergeAttribute("onchange", string.Format("AttachTagToPhoto(this.value, {0})", photo.PhotoID));
                tagsCell.InnerHtml.AppendHtml(tagSelector);                
                TagBuilder tagTable = new TagBuilder("table");
                for (int j = 0; j < photo.PhotoTags.Count(); ++j)
                {                    
                    TagBuilder action = new TagBuilder("a");                
                    action.InnerHtml.Append(photo.PhotoTags[j].Tag.TagPhrase);
                    action.MergeAttribute("href", string.Format("/Tags/TagPhotos/?tagID={0}", photo.PhotoTags[j].TagID));
                    TagBuilder tagCell = new TagBuilder("td");
                    tagCell.InnerHtml.AppendHtml(action);
                    TagBuilder tagRow = new TagBuilder("tr");
                    tagRow.InnerHtml.AppendHtml(tagCell);
                    tagTable.InnerHtml.AppendHtml(tagRow);
                }
                tagsCell.InnerHtml.AppendHtml(tagTable);
                row.InnerHtml.AppendHtml(tagsCell);
                table.InnerHtml.AppendHtml(row);
                TagBuilder secondRow = new TagBuilder("tr"); 
                TagBuilder downloadCell = new TagBuilder("td");
                TagBuilder downloadAction = new TagBuilder("a");
                downloadAction.InnerHtml.Append("Download");
                downloadAction.MergeAttribute("href", string.Format("/Photo/DownloadPhoto?photoID={0}", photo.PhotoID));
                downloadCell.InnerHtml.AppendHtml(downloadAction);
                secondRow.InnerHtml.AppendHtml(downloadCell);
                TagBuilder deleteCell = new TagBuilder("td");
                TagBuilder deleteAction = new TagBuilder("a");
                deleteAction.InnerHtml.Append("Delete");
                deleteAction.MergeAttribute("href", string.Format("/Photo/DeletePhoto?photoID={0}", photo.PhotoID));
                deleteCell.InnerHtml.AppendHtml(deleteAction);
                secondRow.InnerHtml.AppendHtml(deleteCell);
                table.InnerHtml.AppendHtml(secondRow);
            }
            output.Content.AppendHtml(table);
        }

        private TagBuilder CreateTagSelector()
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
            return tagSelector;
        }
    }
}
