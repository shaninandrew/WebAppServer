window.onload = boot();

function boot()
{
 
   
}

//������������ � ������� ����� 
 function toggle_to_fullscreen ()
 {
     if (document.fullscreenEnabled)
     {
         document.documentElement.requestFullscreen();
         console.log("Toggle to fullscreen successfully done");
    }
     else
     {
        console.log("Toggle to fullscreen is disable!");
     }


}

//������������ �� ������� ������ ������
function collapse_fullscreen() {
    
    document.exitFullscreen();    
    console.log("Toggle to shrinkscreen successfully done");

}


