

/**
 * Take care of loading an asset
 */
export const asset = ({ path }) => {
  return require('@/assets/'+ path)
}

/**
 * Take care of loading an icon
 */
export const icon = ({ name }) => {
  return asset({ path: 'icons/' + name });
}

/**
 * Take care of loading an image
 */
export const image = ({ name }) => {
  return asset({ path: 'image/' + name });
}