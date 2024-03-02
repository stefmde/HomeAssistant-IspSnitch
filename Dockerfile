FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy-arm64v8

# Install OS dependencies
RUN apt-get update \
    && apt-get install -y curl gnupg2 \
    && curl -s https://packagecloud.io/install/repositories/ookla/speedtest-cli/script.deb.sh | bash \
    && apt-get install speedtest

# Do this here to set license and gdpr here bc it acts strange in application
RUN speedtest --accept-license --accept-gdpr --progress=no

# BUILD
COPY IspSnitch /build
WORKDIR /build
RUN dotnet restore IspSnitch/IspSnitch.csproj
RUN dotnet build IspSnitch/IspSnitch.csproj -c Release -o output --property WarningLevel=0
RUN cp -a /build/output/. /app
RUN rm -rf /build

# Set executable
WORKDIR /app
RUN chmod +x IspSnitch

# Copy data for add-on
#COPY run.sh /
#RUN chmod a+x /run.sh
#RUN echo $(ls /)
#RUN echo $(ls /app)
#CMD [ "/run.sh" ]
ENTRYPOINT [ "dotnet", "IspSnitch.dll" ]