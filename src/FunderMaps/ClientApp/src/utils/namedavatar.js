
import namedavatar from 'namedavatar'
import avatarColors from 'config/avatarColors'

export const config = () => {
  namedavatar.config({ 
    nameType: 'initials',
    backgroundColors: avatarColors
  })
}

export const generateAvatar = ({ name }) => {
  return namedavatar.getDataURI(
    namedavatar.getSVGString(name)
  )
}

