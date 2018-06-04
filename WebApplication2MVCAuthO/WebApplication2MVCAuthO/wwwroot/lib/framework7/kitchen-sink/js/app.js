// Dom7
var $ = Dom7;

// Theme
var theme = 'auto';
if (document.location.search.indexOf('theme=') >= 0) {
  theme = document.location.search.split('theme=')[1].split('&')[0];
}

// Init App
var app = new Framework7({
  id: 'io.framework7.testapp',
  root: '#app',
  theme: theme,
  data: function () {
    return {
      user: {
        firstName: 'John',
        lastName: 'Doe',
      },
    };
  },
  methods: {
    helloWorld: function () {
      app.dialog.alert('Hello World!');
    },
  },
  routes: routes,
  vi: {
    placementId: 'pltd4o7ibb9rc653x14',
  },
  dialog: {
    // set default title for all dialog shortcuts
    title: 'My App',
    // change default "OK" button text
    buttonOk: '<font size="4">OK</font>'
  }
});

/*
var mainView = app.views.create('.view-main', {
  // These routes are only available in this view
  routesAdd: [
    {
      path: '',
      url: './Home/Index/',
      history: true
    }
  ],
});
  */  

