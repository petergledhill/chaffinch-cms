#!/usr/bin/env bash
cd $(dirname $0)

set -e

docker exec -it chaffinch-dev-dotnet app-test
