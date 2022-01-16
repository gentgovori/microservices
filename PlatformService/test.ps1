if(Get-Command docker)
{
   Write-Host  'Docker is installed!...';
   
   $hub = Read-Host "Please enter your <docker hub>/<image-name>"

   Write-Host  'Building the image...';

   docker build -t $hub .


   Write-Host  'Pushing image to dockerhub';

   docker push $hub 


   Write-Host  'Running container...';

   $port = Read-Host "Please enter your port number: "

   docker run -p $port -d $hub


   Write-Host  'Container is now running...';

   docker ps

   Write-Host  'Press any key to continue...';
   $null =$Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
}
else
{
    Write-Host -NoNewLine 'Docker is not installed!...';
    Write-Host -NoNewLine 'Press any key to continue...';
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
}
