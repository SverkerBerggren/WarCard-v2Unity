for Sprite in ./*.png; do
    if [[ $Sprite =~ .*png$ ]]; then
        cp "$Sprite" "${Sprite}_temp"
        ffmpeg -y -i "${Sprite}_temp" -vf scale=100:-1 "$Sprite"
        rm "${Sprite}_temp"
    fi
done
