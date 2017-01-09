﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using PhotoBank.Models;

namespace PhotoBank.TagHelpers
{
    public class PhotosTagHelper : TagHelper
    {
        public ViewModels.TagsPhotoIndexViewModel photoContent { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tagSelector = new TagBuilder("select");
            var option = new TagBuilder("option");
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
            
            output.TagName = "table";            
            List<Photo> photos = photoContent.Photos.ToList();
            TagBuilder table = new TagBuilder("table");
            TagBuilder row = new TagBuilder("tr");
            TagBuilder cell = new TagBuilder("td");
            TagBuilder image = new TagBuilder("img");            
            for (int i = 0; i < photos.Count; ++i)
            {

                image.InnerHtml.Clear();
                image.Attributes.Clear();
                cell.InnerHtml.Clear();
                var photo = photos[i];
                var base64 = Convert.ToBase64String(photo.Data);
                image.MergeAttribute("src", string.Format("data:image/gif;base64,{0}", base64));
                image.MergeAttribute("style", "height: 100px; width: 100px; border: 4px double black");
                cell.InnerHtml.AppendHtml(image);
                row.InnerHtml.AppendHtml(cell);
                var cell2 = new TagBuilder("td");
                cell2.InnerHtml.Clear();
                cell2.InnerHtml.AppendHtml(tagSelector);
                row.InnerHtml.AppendHtml(cell2);

                //TagBuilder tagsTable = new TagBuilder("table");
                //TagBuilder taqsRow = new TagBuilder("tr");
                //TagBuilder tagCell = new TagBuilder("td");
                //for (int j = 0; j < photo.PhotoTags.Count(); ++j)
                //{
                //    tagCell.InnerHtml.Append(photo.PhotoTags[i])                    
                //}
                table.InnerHtml.AppendHtml(row);
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
    }
}
