all: version
	dotnet build --nologo

version:
	find src -type f -exec sed -i "s/@@VERSION@@/`git describe --long --always`/" {} +
	find src -type f -exec sed -i "s/@@COMMIT@@/`git rev-parse HEAD`/" {} +

test:
	dotnet test

clean:
	dotnet clean --nologo
