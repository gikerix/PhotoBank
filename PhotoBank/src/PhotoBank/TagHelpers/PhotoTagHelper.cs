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
        public int CellsPerRow { get; set; }
        public bool ShowControls { get; set; }
        private List<Photo> photos;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";            
            photos = photoContent.Photos.ToList();            
            int cellsPerRow = 3;
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
                TagBuilder row = new TagBuilder("tr");
                row.InnerHtml.AppendHtml(tableCell);
                TagBuilder tagsCell = new TagBuilder("td");
                tagsCell.MergeAttribute("valign", "top");
                tagsCell.InnerHtml.Clear();
                TagBuilder tagSelector = makeTagSelector();
                tagSelector.Attributes.Clear();
                tagSelector.MergeAttribute("onchange", string.Format("AttachTagToPhoto(this.value, {0})", photo.PhotoID));
                tagsCell.InnerHtml.AppendHtml(tagSelector);
                tagsCell.InnerHtml.AppendHtml(makeTagTable(photo.PhotoTags));
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
            image.MergeAttribute("style", "height: 100px; width: 100px; border: 4px double black; margin:10px");
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
