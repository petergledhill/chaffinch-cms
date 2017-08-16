#!/usr/bin/env bash
cd $(dirname $0)

set -e

docker exec -it chaffinch-core-dev-dotnet app-test
