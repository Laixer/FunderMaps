all: version docker
	dotnet build --nologo

version:
	find src -type f -exec sed -i "s/@@VERSION@@/`git describe --long --always`/" {} +
	find src -type f -exec sed -i "s/@@COMMIT@@/`git rev-parse HEAD`/" {} +

docker:
	docker build --build-arg subtool=FunderMaps.WebApi -t fundermaps-app:latest .
	docker build --build-arg subtool=FunderMaps.Webservice -t fundermaps-webservice:latest .
	docker build --build-arg subtool=FunderMaps.BatchNode -t fundermaps-batch:latest .
	docker build --build-arg subtool=FunderMaps.Portal -t fundermaps-portal:latest .
	docker build --build-arg subtool=FunderMaps.Cli -t fundermaps-cli:latest .

test:
	dotnet test

clean:
	dotnet clean --nologo
	find . -type f -name "Documentation*.xml" -exec rm {} +
