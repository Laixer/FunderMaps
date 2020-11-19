all: protos

protos:
	python3 -m grpc_tools.protoc -I ../../src/FunderMaps.Infrastructure/ --python_out=. --grpc_python_out=. ../../src/FunderMaps.Infrastructure/Protos/*.proto