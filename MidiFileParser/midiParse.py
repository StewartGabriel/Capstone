import mido

def extractMidi(midi_file, song):    
    for i, track in enumerate(midi_file.tracks):        # some midi files have different 'tracks' which need to be played at once                                   
        file_name = f'{song}{i}{track.name}.txt'        # I'll try to find songs that only have right and left hand tracks

        with open(f'./unprocessed_midi/{file_name}', 'w') as midi_messages:                               
            midi_messages.write(f'Track {i}: {track.name}\n')   
            for msg in track:
                midi_messages.write(f'{msg}\n')

        print(f'MIDI messages have been written to {file_name}.')
        processMidi(file_name)

def processMidi(midi_file):
    with open(f'./unprocessed_midi/{midi_file}', 'r') as input:
        messages = input.readlines()
    
    filtered_messages = []
    for line in messages:
        if 'note_on' in line or 'note_off' in line:
            filtered_messages.append(line)

    processed_file_name = f'./processed_midi/processed{midi_file}'
    with open(processed_file_name, 'w')  as output:
        for message in filtered_messages:
            processed_message = message.split(' ')

            msg_type = processed_message[0].split('_')[1]
            note_num = processed_message[2].split('=')[1]
            velocity = processed_message[3].split('=')[1]
            delay_time = processed_message[4].split('=')[1]

            processed_message = msg_type + ' ' + note_num + ' ' + velocity + ' ' + delay_time 
            output.write(processed_message)
    print(f'On/Off messages have been written to {processed_file_name}.')

if __name__ == "__main__":
    midi_file = mido.MidiFile('./midi_files/FurElise.mid') #Change this file path to convert different midi files
    extractMidi(midi_file, 'FurElise') # Change the song name with the file