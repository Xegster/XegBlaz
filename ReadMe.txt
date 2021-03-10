> In visual studio, right-click XegBlaz project and publish
>> Choose the "publish to folder" option
>> Target Location:   publish\

> Open command prompt at XegBlaz directory (same place where XegBlaz.csproj is)

> run command:
firebase login

> run command:
firebase init

> follow prompts. Mostly pretty obvious. For "index.html already exists. Overwrite?" choose no
> For the target directory choose:
publish\wwwroot

> run command:
firebase deploy

project console: https://console.firebase.google.com/project/xegblaz-242c0/overview

publish url: https://xegblaz-242c0.web.app