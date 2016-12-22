//redirecting tag section to controller
function AttachTagToPhoto (tagSelectorValue, photoId){
    window.location = "/Photo/AttachTagToPhoto?tagID=" + tagSelectorValue + "&photoID=" + photoId;
}