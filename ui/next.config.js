/** @type {import('next').NextConfig} */
const nextConfig = {
  async redirects() {
    return [
      {
        source: '/',
        destination: '/dashboard',
        permanent: true,
      },
    ];
  },
  async generateBuildId() {
    return '00000000-0000-0000-0000-000000000000'; // For now an empty guid suffices.
  },
};

module.exports = nextConfig;
