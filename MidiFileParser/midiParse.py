import mido
midi_file = mido.MidiFile('./resources/FurElise.mid')

# Open the text file to write
with open('midi_messages.txt', 'w') as text_file:
    for i, track in enumerate(midi_file.tracks):
        text_file.write(f'Track {i}: {track.name}\n')
        for msg in track:
            text_file.write(f'{msg}\n')
        text_file.write('\n')

print("MIDI messages have been written to 'midi_messages.txt'.")