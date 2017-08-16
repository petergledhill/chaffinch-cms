FROM microsoft/dotnet:2.0-sdk

COPY stage/ /
RUN chmod +x /usr/local/bin/app-*

CMD ["app-bootstrap"]
