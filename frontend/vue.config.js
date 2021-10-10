module.exports = {
  devServer: {
    disableHostCheck: true,
    proxy: {
      '^/api': {
        target: 'http://testhub.local',
        ws: true,
        changeOrigin: true
      },
      '^/uploads': {
        target: 'http://testhub.local',
        ws: true,
        changeOrigin: true
      }
    }
  },

  // output built static files to Laravel's public dir.
  // note the "build" script in package.json needs to be modified as well.
  outputDir: 'dist',

  // modify the location of the generated HTML file.
  // make sure to do this only in production.
  indexPath: process.env.NODE_ENV === 'production'
    ? 'index.html'
    : 'index.html'
}