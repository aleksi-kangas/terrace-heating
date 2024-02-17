/** @type {import('next').NextConfig} */
const nextConfig = {
  output: 'standalone',
  redirects: async () => {
    return [
      {
        source: '/',
        destination: '/dashboard',
        permanent: true,
      },
    ];
  },
  generateBuildId: async () => {
    return '00000000-0000-0000-0000-000000000000'; // For now an empty guid suffices.
  },
};

module.exports = nextConfig;
