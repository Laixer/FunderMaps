all: version docker
	dotnet build --nologo

version:
	find src -type f -exec sed -i "s/@@VERSION@@/`git describe --long --always`/" {} +
	find src -type f -exec sed -i "s/@@COMMIT@@/`git rev-parse HEAD`/" {} +

docker:
	docker build -t fundermaps . --file Dockerfile

test:
	dotnet test

clean:
	dotnet clean --nologo
