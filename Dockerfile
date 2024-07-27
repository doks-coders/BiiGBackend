#FROM mcr.microsoft.com/dotnet/sdk:8.0.100 AS build-env
FROM mcr.microsoft.com/dotnet/sdk:8.0.100 AS build-env
WORKDIR /app
EXPOSE 8080
#add the work directory.
#And this sets the directory that the container starts in.
#And the main purpose of this is that any future commands that we execute inside this file happen inside
#this working directory.


# copy csproj and restore as distinct layers
# Copy all .csproj files from each project into the container
COPY ./*.csproj ./
COPY ./BiiGBackend.ApplicationCore/*.csproj ./BiiGBackend.ApplicationCore/
COPY ./BiiGBackend.Infrastructure/*.csproj ./BiiGBackend.Infrastructure/
COPY ./BiiGBackend.Models/*.csproj ./BiiGBackend.Models/
COPY ./BiiGBackend.StaticDefinitions/*.csproj ./BiiGBackend.StaticDefinitions/
COPY ./BiiGBackend.Web/*.csproj ./BiiGBackend.Web/

# Repeat this for any other projects

# Restore dependencies for each project
RUN dotnet restore ./BiiGBackend.ApplicationCore/BiiGBackend.ApplicationCore.csproj
RUN dotnet restore ./BiiGBackend.Infrastructure/BiiGBackend.Infrastructure.csproj
RUN dotnet restore ./BiiGBackend.Models/BiiGBackend.Models.csproj
RUN dotnet restore ./BiiGBackend.StaticDefinitions/BiiGBackend.StaticDefinitions.csproj
RUN dotnet restore ./BiiGBackend.Web/BiiGBackend.Web.csproj
# Repeat this for any other projects

# Copy the rest of the files




#And then inside our working directory, we're going to run dotnet restore  against that csproj  file, 

#and that restores all of our dependencies that are listed inside there.


# copy everything else and build
COPY . ./

#that's going to take all of our content inside our projects and copy that into our working 
#directory.      


RUN dotnet publish -c Release -o out
#And we're going to create a configuration that's called release.
#And then we specify the directory we want to put this out into.

# build runtime image
#We can use a smaller version and we can just use the runtime instead at this point.

#So we need the SDK to run the 'net restore' command and a 'dotnet publish' commands.

#But once we've done that, we can then go and use a smaller image that just contains the runtime 
#instead of the full SDK.

FROM mcr.microsoft.com/dotnet/aspnet:8.0

#And then because we're using the new image, we need to specify the work directory again 
#and keep it as  /app.
WORKDIR /app

# And then we're going to copy everything that's contained in our build environment that 
# we were using up here. And we specified that stash --from=build-env. And then we're going 
# to say that we're going to copy that from the /app/out into . , which is the root of 
# our container. 

# So everything that's inside that "out" directory that we created here is going to be copied 
# to the root of our container.

#docker build -t guonnie/tutorapplication .
#docker run --rm -it -p 8080:8080 guonnie/tutorapplication:latest
#docker push guonnie/locationquestions:latest

#docker build -t guonnie/locationquestions .
#docker run --rm -it -p 8080:8080 guonnie/locationquestions:latest
#docker push guonnie/locationquestions:latest
#C:\Users\HP\Downloads\flyctl-0.2.4\flyctl-0.2.4\bin\flyctl.exe launch --image guonnie/locationquestions:latest

COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "BiiGBackend.Web.dll" ]

#So if you decided to call your project something other than API at this point, 
#you would need to tell

#it what you called it. I called mine API.

#So that's what I'm putting inside here because that's what's going to be generated when 
#we publish our application, we're going to have an API DLL.

#And that's the entry point to our application. It looks inside the program class.
#It executes that code in there and then runs our application.



#docker build --no-cache -t your_image_name .

#docker build -t guonnie/dateapp .

# docker run --rm -it -p 8080:8080 guonnie/datingapp:latest

# docker exec -it guonnie/datingapp ls /app
# docker history guonnie/datingapp