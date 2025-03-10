FROM unitymultiplay/linux-base-image:latest

# copy game files here
# for example:
WORKDIR /game
COPY --chown=mpukgame . .

# set your game binary as the entrypoint
ENTRYPOINT [ "./gamebinary" ]
