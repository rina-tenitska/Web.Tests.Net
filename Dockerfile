# Extend Jenkins 2.249.2 on Debian 9 (stretch)
FROM jenkins/jenkins:latest

# Switch to root user to install .NET SDK
USER root

# Needed dependencies...
RUN apt install -y libunwind8 libicu57

# Switch back to jenkins user
USER jenkins