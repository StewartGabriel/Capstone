if __name__ == '__main__':
    # Change this to the name of the midi files you want to compare
    left_hand = 'processed_midi\FurElise\processedFurElise2Piano left.txt' # Change this to the left hand file name
    right_hand = 'processed_midi\FurElise\processedFurElise1Piano right.txt' # Change this to the right hand file name

    left_time = 0 # Keeps track of the time the left hand has been playing
    left_line = 0 # Keeps track of the line number of the left hand file
    right_time = 0 # Keeps track of the time the right hand has been playing
    right_line = 0 # Keeps track of the line number of the right hand file

    with open(left_hand, 'r') as left_file, open(right_hand, 'r') as right_file:
        left_messages = left_file.readlines()
        right_messages = right_file.readlines()
        for i in range(len(left_messages)):
            # get the current left hand note and its status
            left_message = left_messages[i]
            left_line += 1
            left_message_time = int(left_message.split(' ')[3]) # Gets the delay time of the current note
            left_time += left_message_time # Adds the delay time of each note to the left hand time
            current_left_note = left_message.split(' ')[1] # Gets the note number of the current left hand note
            current_left_note_status = left_message.split(' ')[0] # Gets the left note's on/off status

            # print(f'Left hand note {current_left_note} is played at time {left_time} for {left_message_time}ms with status {current_left_note_status}.\tleft: {left_line}')

            while right_time < left_time and right_line < len(right_messages):

                # get the current right hand note and its status
                right_message = right_messages[right_line]
                right_line += 1
                right_message_time = int(right_message.split(' ')[3]) # Gets the delay time of the current note
                right_time += right_message_time # Adds the delay time of each note to the right hand time
                current_right_note = right_message.split(' ')[1]
                current_right_note_status = right_message.split(' ')[0]

                print(f'Right hand note {current_right_note} is played at time {right_time} for {right_message_time}ms with status {current_right_note_status}.\tright: {right_line}')

                if left_time == right_time and current_left_note_status == current_right_note_status: # and current_left_note == current_right_note:
                    print(f'Left hand note {current_left_note} and right hand note {current_right_note} are played at the same time\tleft: {left_line} {left_time}ms, right: {right_line} {right_time}ms.')
                    break
                elif left_time < right_time:
                    break
