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
                //TagBuilder dowloadCell = new TagBuilder("td");
                //TagBuilder downloadAction = new TagBuilder("a");
                //dou
                table.InnerHtml.AppendHtml(row);
                
                //TagBuilder deleteCell = new TagBuilder("td");
            }
            output.Content.AppendHtml(table);            
            
            //output.TagName = "table";
            //string rows = string.Empty;

            //{
            //    rows += "<tr>";
            //    rows += "<td >"
            //    @name < br />
            //    < img src = "@imgSrc" style = "height:100px; width:100px; border:4px double black" />

            //   </ td >

            //   < td valign = "top" >
            //        @if(photo.PhotoTags.Count > 0)
            //    {
            //        < table >
            //            @foreach(var tag in photo.PhotoTags)
            //            {
            //            < tr >
            //                < td style = "padding-left:10px" >
            //                     @Html.ActionLink(tag.Tag.TagPhrase, "TagPhotos", "Tags", new { tagID = tag.Tag.TagID })
            //                 </ td >

            //             </ tr >
            //            }
            //        </ table >
            //    }
            //    < select asp - items = "@Model.TagSelectionList" style = "margin-left:10px" id = "tagList" onchange = "AttachTagToPhoto(this.value, @id)" >

            //                 < option > Select tag </ option >

            //                </ select >

            //            </ td >

            //            < td >
            //                @Html.ActionLink("Download Photo", "DownloadPhoto", new { photoID = id })
            //            </ td >

            //        </ tr >

            //        < tr >

            //            < td >
            //                @Html.ActionLink("Delete", "DeletePhoto", new { photoID = id })
            //            </ td >

            //        </ tr >

            //    </ table >

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
