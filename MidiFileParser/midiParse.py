import mido
import os

def extractMidi(midi_file, song_name):    
    for i, track in enumerate(midi_file.tracks):        # some midi files have different 'tracks' which need to be played at once                                   
        file_name = repr(f'{song_name}{i}{track.name}.txt').replace("\\x00", "")
        file_name = file_name.replace("\'\'", "")        # I'll try to find songs that only have right and left hand tracks
        print(repr(f'./unprocessed_midi/{song_name}/{file_name}'))
        with open(f'./unprocessed_midi/{song_name}/{file_name}', 'w') as midi_messages:                               
            midi_messages.write(f'Track {i}: {track.name}\n')   
            for msg in track:
                midi_messages.write(f'{msg}\n')

        print(f'MIDI messages have been written to {file_name}.')
        processMidi(file_name)

def processMidi(midi_file):
    with open(f'./unprocessed_midi/{song_name}/{midi_file}', 'r') as input:
        messages = input.readlines()
    
    filtered_messages = []
    for line in messages:
        if 'note_on' in line or 'note_off' in line:
            filtered_messages.append(line)
        processed_file_name = f'./processed_midi/{song_name}/processed{midi_file}'
        with open(processed_file_name, 'w')  as output:
            for message in filtered_messages: # Extract useful information from each segment
                processed_message = message.split(' ')
                msg_type = processed_message[0].split('_')[1]
                note_num = processed_message[2].split('=')[1]
                velocity = processed_message[3].split('=')[1]
                delay_time = processed_message[4].split('=')[1]

                if velocity == '0': # Ensures messages with 0 velocity are note_off  
                    msg_type = 'off'

                processed_message = msg_type + ' ' + note_num + ' ' + velocity + ' ' + delay_time 
                output.write(processed_message)

        if os.path.getsize(processed_file_name) == 0: # Delete empty files (try to, at least)
            os.remove(processed_file_name)
    print(f'On/Off messages have been written to {processed_file_name}.\n')
    
if __name__ == '__main__':
    song_name = 'NuclearFusion' # Put the midi file into the folder ./midi_files and write the file name here without the extension
    if not os.path.exists(f'./processed_midi/{song_name}') or not os.path.exists(f'./unprocessed_midi/{song_name}'): # Creates a folder to store the data
        os.makedirs(f'./processed_midi/{song_name}', exist_ok=True)
        os.makedirs(f'./unprocessed_midi/{song_name}', exist_ok=True)

    midi_file = mido.MidiFile(f'./midi_files/{song_name}.mid') 
    try:
        extractMidi(midi_file, song_name)
    except OSError:
        print('File has a strange track name')