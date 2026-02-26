SHELL := /bin/bash

SLN := Zireael.DotNetBinding.slnx
UPSTREAM_SUBMODULE := upstream-zireael

.DEFAULT_GOAL := help

.PHONY: help sync-upstream print-upstream-sha build-native build test run-sample sample update update-test

help:
	@echo "Targets:"
	@echo "  make sync-upstream    # fetch latest RtlZeroMemory/Zireael into upstream-zireael submodule"
	@echo "  make print-upstream-sha # show currently pinned upstream commit SHA"
	@echo "  make build-native     # build and stage native zireael library"
	@echo "  make build            # build .NET solution"
	@echo "  make test             # run .NET tests"
	@echo "  make run-sample       # run sample with native library path setup"
	@echo "  make sample           # one-command setup: build native + run sample"
	@echo "  make update           # sync upstream + build native + build managed"
	@echo "  make update-test      # update + run tests"

sync-upstream:
	git submodule sync -- $(UPSTREAM_SUBMODULE)
	git submodule update --init --recursive $(UPSTREAM_SUBMODULE)
	git submodule update --remote -- $(UPSTREAM_SUBMODULE)
	@echo "Pinned upstream SHA: $$(git -C $(UPSTREAM_SUBMODULE) rev-parse --short HEAD)"

print-upstream-sha:
	@echo "$$(git -C $(UPSTREAM_SUBMODULE) rev-parse --short HEAD)"

build-native:
	./eng/build-native.sh

build:
	dotnet build $(SLN)

test:
	dotnet test $(SLN) --nologo

run-sample:
	./eng/run-sample.sh

sample:
	./eng/sample.sh

update: sync-upstream build-native build

update-test: update test
